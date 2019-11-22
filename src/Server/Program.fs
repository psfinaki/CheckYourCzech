module Program

open System
open System.IO
open Microsoft.Extensions.DependencyInjection
open Saturn
open Giraffe.Serialization.Json
open Thoth.Json.Giraffe

let GetEnvVar var =
    match Environment.GetEnvironmentVariable(var) with
    | null -> None
    | value -> Some value

let publicPath = GetEnvVar "public_path" |> Option.defaultValue "../Client/public" |> Path.GetFullPath
let port = 8085us

let configureSerialization (services:IServiceCollection) =
    Storage.configTypeDescriptor() |> ignore
    services.AddSingleton<IJsonSerializer>(ThothSerializer())
    

let configureAzure (services:IServiceCollection) =
    GetEnvVar "APPINSIGHTS_INSTRUMENTATIONKEY"
    |> Option.map services.AddApplicationInsightsTelemetry
    |> Option.defaultValue services

let app = application {
    url ("https://0.0.0.0:" + port.ToString() + "/")
    force_ssl
    use_router WebServer.webApp
    memory_cache
    use_static publicPath
    service_config configureSerialization
    service_config configureAzure
    use_gzip
}

run app
