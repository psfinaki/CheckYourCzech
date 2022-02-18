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

let runDotNetWithDir cmd workingDir =
    DotNet.exec (DotNet.Options.withWorkingDirectory workingDir) cmd "" |> ignore

let runDotNet cmd = runDotNetWithDir cmd "."

let serverWatcher() = 
    async {
        runDotNetWithDir "watch run" serverPath
    }

let clientWatcher openBrowser = 
    async {
        let webpackCommand = openBrowser |> function
            | Open -> $"webpack-dev-server --config {webpackDevConfig} --open"
            | DontOpen -> $"webpack-dev-server --config {webpackDevConfig}"

        let fableCommand = $"watch src/Client -o src/Client/fable --run {webpackCommand}"
        DotNet.exec id "fable" fableCommand |> ignore
    }

let setEnvironmentVariables() =
    Environment.setEnvironVar "ASPNETCORE_ENVIRONMENT" "local"
    Environment.setEnvironVar "STORAGE_CONNECTIONSTRING" "UseDevelopmentStorage=true"
    Environment.setEnvironVar "SERVER_URL" "http://localhost:8080/"
    
let restoreTools() = runDotNet "tool restore"

let restorePackages() = 
    runDotNet "paket restore"

Target.create "Build" (fun _ ->
    setEnvironmentVariables()
    restoreTools()
    restorePackages()
    
    runDotNet "build"
    
    let webpackCommand = $"webpack --config {webpackProdConfig}"
    let fableCommand = $"src/Client -o src/Client/fable --run {webpackCommand}"
    DotNet.exec id "fable" fableCommand |> ignore
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
    runDotNetWithDir "run" scraperPath
)

Target.create "RunE2ETests" (fun _ ->
    runDotNetWithDir "test" clientTestsPath
)

Target.create "RunWebWithE2ETests" (fun _ ->
    let server = serverWatcher()
    let client = clientWatcher Browser.DontOpen

    let serverTask = 
        [ server; client ] 
        |> Async.Parallel 
        |> Async.StartAsTask
    sleep 15000 |> Async.RunSynchronously

    runDotNetWithDir "test" clientTestsPath
    killClientServerProc()

    serverTask 
    |> Async.AwaitTask 
    |> Async.Catch
    |> Async.Ignore
    |> Async.RunSynchronously
)

"Build" ==> "RunWeb"
"Build" ==> "RunE2ETests"
"Build" ==> "RunWebWithE2ETests"
"Build" ==> "RunScraper"

Target.runOrDefaultWithArguments "Build"
