#r "paket: groupref Build //"
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.DotNet
open Fake.Core
open Fake.Core.TargetOperators
open Fake.IO

type Browser =
    | Open
    | DontOpen

let serverPath = Path.getFullName "./src/Server"
let clientPath = Path.getFullName "./src/Client"
let scraperPath = Path.getFullName "./src/Scraper"
let clientTestsPath = Path.getFullName "./tests/Client.UiTests"
let webpackDevConfig = Path.combine clientPath "webpack.development.js"
let webpackProdConfig = Path.combine clientPath "webpack.production.js"

let killClientServerProc() = 
    Process.killAllByName "dotnet"
    Process.killAllByName "dotnet.exe"
    Process.killAllByName "node"

let sleep ms = 
    async {
        do! Async.Sleep(ms)
    }

let yarnTool =
    let tool = if Environment.isUnix then "yarn" else "yarn.cmd"
    match ProcessUtils.tryFindFileOnPath tool with
    | Some t -> t
    | _ ->
        let errorMsg =
            tool + " was not found in path. " +
            "Please install it and make sure it's available from your path. " +
            "See https://safe-stack.github.io/docs/quickstart/#install-pre-requisites for more info"
        failwith errorMsg

let yarn yarnCmd = 
    let arguments = yarnCmd |> String.split ' ' |> Arguments.OfArgs
    Command.RawCommand (yarnTool, arguments)
    |> CreateProcess.fromCommand
    |> CreateProcess.ensureExitCode
    |> Proc.run
    |> ignore

let runDotNet cmd workingDir =
    let result =
        DotNet.exec (DotNet.Options.withWorkingDirectory workingDir) cmd ""
    if result.ExitCode <> 0 then failwithf "'dotnet %s' failed in %s" cmd workingDir

let serverWatcher() = 
    async {
        runDotNet "watch run" serverPath
    }

let clientWatcher openBrowser = 
    async {
        let webpackCommand = openBrowser |> function
            | Open -> sprintf "webpack-dev-server --config %s --open" webpackDevConfig
            | DontOpen -> sprintf "webpack-dev-server --config %s" webpackDevConfig
        
        yarn webpackCommand
    }

Target.create "SetEnvironmentVariables" (fun _ ->
    Environment.setEnvironVar "ASPNETCORE_ENVIRONMENT" "local"
    Environment.setEnvironVar "STORAGE_CONNECTIONSTRING" "UseDevelopmentStorage=true"
    Environment.setEnvironVar "SERVER_URL" "http://localhost:8080/"
)

Target.create "InstallClient" (fun _ ->
    printfn "Yarn version:"
    yarn "--version"
    yarn "install"
    runDotNet "restore" clientPath
)

Target.create "Build" (fun _ ->
    runDotNet "build" serverPath

    let webpackCommand = sprintf "webpack --config %s" webpackProdConfig
    yarn webpackCommand
)

Target.create "RunWeb" (fun _ ->
    let server = serverWatcher()
    let client = clientWatcher Browser.Open

    [ server; client ]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
)

Target.create "RunScraper" (fun _ ->
    runDotNet "run" scraperPath
)

Target.create "RunE2ETests" (fun _ ->
    runDotNet "test" clientTestsPath
)

Target.create "RunWebWithE2ETests" (fun _ ->
    let server = serverWatcher()
    let client = clientWatcher Browser.DontOpen

    let serverTask = 
        [ server; client ] 
        |> Async.Parallel 
        |> Async.StartAsTask
    sleep 15000 |> Async.RunSynchronously

    runDotNet "test" clientTestsPath
    killClientServerProc()

    serverTask 
    |> Async.AwaitTask 
    |> Async.Catch
    |> Async.Ignore
    |> Async.RunSynchronously
)

"InstallClient"
    ==> "Build"

"SetEnvironmentVariables"
    ==> "InstallClient"
    ==> "RunWeb"

"SetEnvironmentVariables"
    ==> "RunE2ETests"

"SetEnvironmentVariables"
    ==> "InstallClient"
    ==> "RunWebWithE2ETests"

"SetEnvironmentVariables"
    ==> "RunScraper"

Target.runOrDefaultWithArguments "Build"
