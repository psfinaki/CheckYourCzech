#r "paket: groupref build //"
#load "./.fake/build.fsx/intellisense.fsx"

#if !FAKE
#r "netstandard"
#r "Facades/netstandard" // https://github.com/ionide/ionide-vscode-fsharp/issues/839#issuecomment-396296095
#endif

#load @"paket-files/build/CompositionalIT/fshelpers/src/FsHelpers/ArmHelper/ArmHelper.fs"

open System
open System.IO
open System.Net

open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators

open Cit.Helpers.Arm
open Cit.Helpers.Arm.Parameters
open Microsoft.Azure.Management.ResourceManager.Fluent.Core

let serverPath = Path.getFullName "./src/Server"
let clientPath = Path.getFullName "./src/Client"
let deployDir = Path.getFullName "./deploy"

let platformTool tool winTool =
    let tool = if Environment.isUnix then tool else winTool
    match ProcessUtils.tryFindFileOnPath tool with
    | Some t -> t
    | _ ->
        let errorMsg =
            tool + " was not found in path. " +
            "Please install it and make sure it's available from your path. " +
            "See https://safe-stack.github.io/docs/quickstart/#install-pre-requisites for more info"
        failwith errorMsg

let nodeTool = platformTool "node" "node.exe"
let yarnTool = platformTool "yarn" "yarn.cmd"

let runTool cmd args workingDir =
    let arguments = args |> String.split ' ' |> Arguments.OfArgs
    Command.RawCommand (cmd, arguments)
    |> CreateProcess.fromCommand
    |> CreateProcess.withWorkingDirectory workingDir
    |> CreateProcess.ensureExitCode
    |> Proc.run
    |> ignore

let runDotNet cmd workingDir =
    let result =
        DotNet.exec (DotNet.Options.withWorkingDirectory workingDir) cmd ""
    if result.ExitCode <> 0 then failwithf "'dotnet %s' failed in %s" cmd workingDir

let openBrowser url =
    //https://github.com/dotnet/corefx/issues/10361
    Command.ShellCommand url
    |> CreateProcess.fromCommand
    |> CreateProcess.ensureExitCodeWithMessage "opening browser failed"
    |> Proc.run
    |> ignore

Target.create "Clean" (fun _ ->
    Shell.cleanDirs [deployDir]
)

Target.create "InstallClient" (fun _ ->
    printfn "Node version:"
    runTool nodeTool "--version" __SOURCE_DIRECTORY__
    printfn "Yarn version:"
    runTool yarnTool "--version" __SOURCE_DIRECTORY__
    runTool yarnTool "install --frozen-lockfile" __SOURCE_DIRECTORY__
    runDotNet "restore" clientPath
)

Target.create "RestoreServer" (fun _ ->
    runDotNet "restore" serverPath
)

Target.create "Build" (fun _ ->
    runDotNet "build" serverPath
    runDotNet "fable webpack-cli -- --config src/Client/webpack.config.js -p" clientPath
)

Target.create "Run" (fun _ ->
    let server = async {
        runDotNet "watch run" serverPath
    }
    let client = async {
        runDotNet "fable webpack-dev-server -- --config src/Client/webpack.config.js" clientPath
    }
    let browser = async {
        do! Async.Sleep 5000
        openBrowser "http://localhost:8080"
    }

    [ server; client; browser ]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
)

Target.create "Bundle" (fun _ ->
    runDotNet (sprintf "publish \"%s\" -c release -o \"%s\"" serverPath deployDir) __SOURCE_DIRECTORY__
    Shell.copyDir (Path.combine deployDir "public") (Path.combine clientPath "public") FileFilter.allFiles
)

type ArmOutput = { WebAppPassword : ParameterValue<string> }

let mutable deploymentOutputs : ArmOutput option = None

Target.create "ArmTemplate" (fun args ->
    let appName = args.Context.Arguments |> List.exactlyOne
    let armTemplate = @"arm-template.json"
    let resourceGroupName = appName

    let authCtx =
        let subscriptionId = Guid.Parse "f400689a-038c-49be-a6ee-2d98e5000d90"
        let clientId = Guid.Parse "71312e3d-43da-424d-9e9d-0829289b4866"

        Trace.tracefn "Deploying template '%s' to resource group '%s' in subscription '%O'..." armTemplate resourceGroupName subscriptionId
        subscriptionId
        |> authenticateDevice Trace.trace { ClientId = clientId; TenantId = None }
        |> Async.RunSynchronously

    let deployment =
        { DeploymentName = appName
          ResourceGroup = New(resourceGroupName, Region.Create Region.EuropeNorth.Name)
          ArmTemplate = File.ReadAllText armTemplate
          Parameters = Simple [ "appName", ArmString appName ]
          DeploymentMode = Incremental }

    deployment
    |> deployWithProgress authCtx
    |> Seq.iter(function
        | DeploymentInProgress (state, operations) -> Trace.tracefn "State is %s, completed %d operations." state operations
        | DeploymentError (statusCode, message) -> Trace.traceError <| sprintf "DEPLOYMENT ERROR: %s - '%s'" statusCode message
        | DeploymentCompleted d -> deploymentOutputs <- d)
)

// https://github.com/SAFE-Stack/SAFE-template/issues/120
// https://stackoverflow.com/a/6994391/3232646
type TimeoutWebClient() =
    inherit WebClient()
    override this.GetWebRequest uri =
        let request = base.GetWebRequest uri
        request.Timeout <- 30 * 60 * 1000
        request

Target.create "AppService" (fun args ->
    let appName = args.Context.Arguments |> List.exactlyOne
    let zipFile = "deploy.zip"
    File.Delete zipFile
    Zip.zip deployDir zipFile !!(deployDir + @"\**\**")

    let appPassword = deploymentOutputs.Value.WebAppPassword.value
    let destinationUri = sprintf "https://%s.scm.azurewebsites.net/api/zipdeploy" appName
    let client = new TimeoutWebClient(Credentials = Net.NetworkCredential("$" + appName, appPassword))
    Trace.tracefn "Uploading %s to %s" zipFile destinationUri
    client.UploadData(destinationUri, File.ReadAllBytes zipFile) |> ignore)

"Clean"
    ==> "InstallClient"
    ==> "Build"
    ==> "Bundle"
    ==> "ArmTemplate"
    ==> "AppService"

"Clean"
    ==> "InstallClient"
    ==> "RestoreServer"
    ==> "Run"

Target.runOrDefaultWithArguments "Build"
