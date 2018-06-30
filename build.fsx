#r @"packages/build/FAKE/tools/FakeLib.dll"
#r "netstandard"
#I "packages/build/Microsoft.Rest.ClientRuntime.Azure/lib/net452"
#load ".paket/load/netcoreapp2.1/Build/build.group.fsx"
#load @"paket-files/build/CompositionalIT/fshelpers/src/FsHelpers/ArmHelper/ArmHelper.fs"

open Cit.Helpers.Arm
open Cit.Helpers.Arm.Parameters
open Microsoft.Azure.Management.ResourceManager.Fluent.Core
open System
open System.IO
open System.Net
open Fake

let appName = "check-your-czech"

let serverPath = "./src/Server" |> FullName
let clientPath = "./src/Client" |> FullName
let deployDir = "./deploy" |> FullName

let platformTool tool winTool =
    let tool = if isUnix then tool else winTool
    match tryFindFileOnPath tool with Some t -> t | _ -> failwithf "%s not found" tool

let nodeTool = platformTool "node" "node.exe"
let yarnTool = platformTool "yarn" "yarn.cmd"

let dotnetcliVersion = DotNetCli.GetDotNetSDKVersionFromGlobalJson()
let mutable dotnetCli = "dotnet"

let run cmd args workingDir =
    let result =
        ExecProcess (fun info ->
            info.FileName <- cmd
            info.WorkingDirectory <- workingDir
            info.Arguments <- args) TimeSpan.MaxValue
    if result <> 0 then failwithf "'%s %s' failed" cmd args

Target "Clean" (fun _ ->
    CleanDirs [deployDir]
)

Target "InstallDotNetCore" (fun _ ->
    dotnetCli <- DotNetCli.InstallDotNetSDK dotnetcliVersion
)

Target "InstallClient" (fun _ ->
    printfn "Node version:"
    run nodeTool "--version" __SOURCE_DIRECTORY__
    printfn "Yarn version:"
    run yarnTool "--version" __SOURCE_DIRECTORY__
    run yarnTool "install --frozen-lockfile" __SOURCE_DIRECTORY__
    run dotnetCli "restore" clientPath
)

Target "RestoreServer" (fun () ->
    run dotnetCli "restore" serverPath
)

Target "Build" (fun () ->
    run dotnetCli "build" serverPath
    run dotnetCli "fable webpack -- -p" clientPath
)

Target "Run" (fun () ->
    let server = async {
        run dotnetCli "watch run" serverPath
    }
    let client = async {
        run dotnetCli "fable webpack-dev-server" clientPath
    }
    let browser = async {
        Threading.Thread.Sleep 5000
        Diagnostics.Process.Start "http://localhost:8080" |> ignore
    }

    [ server; client; browser ]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
)

Target "Bundle" (fun () ->
    run dotnetCli (sprintf "publish \"%s\" -c release -o \"%s\"" serverPath deployDir) __SOURCE_DIRECTORY__
    CopyDir (deployDir </> "public") (clientPath </> "public") allFiles
)

type ArmOutput = { WebAppPassword : ParameterValue<string> }

let mutable deploymentOutputs : ArmOutput option = None

Target "ArmTemplate" (fun _ ->
    let armTemplate = @"arm-template.json"
    let resourceGroupName = appName

    let authCtx =
        let subscriptionId = Guid.Parse "f400689a-038c-49be-a6ee-2d98e5000d90"
        let clientId = Guid.Parse "71312e3d-43da-424d-9e9d-0829289b4866"

        tracefn "Deploying template '%s' to resource group '%s' in subscription '%O'..." armTemplate resourceGroupName subscriptionId
        subscriptionId
        |> authenticateDevice trace { ClientId = clientId; TenantId = None }
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
        | DeploymentInProgress (state, operations) -> tracefn "State is %s, completed %d operations." state operations
        | DeploymentError (statusCode, message) -> traceError <| sprintf "DEPLOYMENT ERROR: %s - '%s'" statusCode message
        | DeploymentCompleted d -> deploymentOutputs <- d)
)

// https://stackoverflow.com/a/6994391/3232646
type WebClient'() = 
    inherit WebClient()
    override this.GetWebRequest uri = 
        let request = base.GetWebRequest uri
        request.Timeout <- 30 * 60 * 1000
        request

Target "AppService" (fun _ ->
    let zipFile = "deploy.zip"
    File.Delete zipFile
    Zip deployDir zipFile !!(deployDir + @"\**\**")

    let appPassword = deploymentOutputs.Value.WebAppPassword.value
    let destinationUri = sprintf "https://%s.scm.azurewebsites.net/api/zipdeploy" appName
    let client = new WebClient'(Credentials = Net.NetworkCredential("$" + appName, appPassword))
    tracefn "Uploading %s to %s" zipFile destinationUri
    client.UploadData(destinationUri, IO.File.ReadAllBytes zipFile) |> ignore)

"Clean"
    ==> "InstallDotNetCore"
    ==> "InstallClient"
    ==> "Build"
    ==> "Bundle"
    ==> "ArmTemplate"
    ==> "AppService"

"InstallClient"
    ==> "RestoreServer"
    ==> "Run"

RunTargetOrDefault "Build"
