#r "nuget: Fake.Core.Target"
#r "nuget: Fake.DotNet.Cli"
#r "nuget: Fake.JavaScript.Yarn"
#r "nuget: System.Reactive" // https://github.com/fsharp/FAKE/issues/2517#issuecomment-727282959

open Fake.DotNet
open Fake.Core
open Fake.Core.TargetOperators
open Fake.IO
open Fake.JavaScript
open System

Environment.GetCommandLineArgs() 
|> Array.skip 2 // fsi.exe; build.fsx
|> Array.toList
|> Context.FakeExecutionContext.Create false __SOURCE_FILE__
|> Context.RuntimeContext.Fake
|> Context.setExecutionContext

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

let sleep (ms: int) = 
    async {
        do! Async.Sleep(ms)
    }

let runDotNet cmd workingDir =
    let result =
        DotNet.exec (DotNet.Options.withWorkingDirectory workingDir) cmd ""
    if result.ExitCode <> 0 then failwith $"'dotnet {cmd}' failed in {workingDir}"

let serverWatcher() = 
    async {
        runDotNet "watch run" serverPath
    }

let clientWatcher openBrowser = 
    async {
        let webpackCommand = openBrowser |> function
            | Open -> $"webpack-dev-server --config {webpackDevConfig} --open"
            | DontOpen -> $"webpack-dev-server --config {webpackDevConfig}"
        
        Yarn.exec webpackCommand id
    }

Target.create "SetEnvironmentVariables" (fun _ ->
    Environment.setEnvironVar "ASPNETCORE_ENVIRONMENT" "local"
    Environment.setEnvironVar "STORAGE_CONNECTIONSTRING" "UseDevelopmentStorage=true"
    Environment.setEnvironVar "SERVER_URL" "http://localhost:8080/"
)

Target.create "InstallClient" (fun _ ->
    printfn "Yarn version:"
    Yarn.exec "--version" id
    Yarn.install id
    runDotNet "restore" clientPath
)

Target.create "Build" (fun _ ->
    runDotNet "build" serverPath

    let webpackCommand = $"webpack --config {webpackProdConfig}"
    Yarn.exec webpackCommand id
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
