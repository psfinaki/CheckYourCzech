#r "paket: groupref build //"
#load "./.fake/build.fsx/intellisense.fsx"

#if !FAKE
#r "netstandard"
#r "Facades/netstandard" // https://github.com/ionide/ionide-vscode-fsharp/issues/839#issuecomment-396296095
#endif

#load @"paket-files/build/CompositionalIT/fshelpers/src/FsHelpers/ArmHelper/ArmHelper.fs"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO

let serverPath = Path.getFullName "./src/Server"
let clientPath = Path.getFullName "./src/Client"
let scraperPath = Path.getFullName "./src/Scraper"

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

Target.create "SetEnvironmentVariables" (fun _ ->
    Environment.setEnvironVar "ASPNETCORE_ENVIRONMENT" "local"
    Environment.setEnvironVar "STORAGE_CONNECTIONSTRING" "UseDevelopmentStorage=true"
)

Target.create "InstallClient" (fun _ ->
    printfn "Yarn version:"
    yarn "--version"
    yarn "install"
    runDotNet "restore" clientPath
)

Target.create "RestoreServer" (fun _ ->
    runDotNet "restore" serverPath
)

Target.create "Build" (fun _ ->
    runDotNet "build" serverPath

    let webpackConfig = Path.combine clientPath "webpack.production.js"
    let webpackCommand =  "webpack --config " + webpackConfig
    yarn webpackCommand
)

Target.create "RunWeb" (fun _ ->
    let server = async {
        runDotNet "watch run" serverPath
    }
    let client = async {
        let webpackConfig = Path.combine clientPath "webpack.development.js"
        let webpackCommand = sprintf "webpack-dev-server --config %s --open" webpackConfig
        yarn webpackCommand
    }

    [ server; client ]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
)

Target.create "RunScraper" (fun _ ->
    runDotNet "run" scraperPath
)

"InstallClient"
    ==> "Build"

"SetEnvironmentVariables"
    ==> "InstallClient"
    ==> "RestoreServer"
    ==> "RunWeb"

"SetEnvironmentVariables"
    ==> "RunScraper"

Target.runOrDefaultWithArguments "Build"
