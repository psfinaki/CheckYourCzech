module Program

open System
open System.IO
open Microsoft.Extensions.DependencyInjection
open Giraffe.Serialization
open Saturn

let GetEnvVar var =
    match Environment.GetEnvironmentVariable(var) with
    | null -> None
    | value -> Some value

let publicPath = GetEnvVar "public_path" |> Option.defaultValue "../Client/public" |> Path.GetFullPath
let port = 8085us

let configureSerialization (services:IServiceCollection) =
    let fableJsonSettings = Newtonsoft.Json.JsonSerializerSettings()
    fableJsonSettings.Converters.Add(Fable.JsonConverter())
    services.AddSingleton<IJsonSerializer>(NewtonsoftJsonSerializer fableJsonSettings)

let configureAzure (services:IServiceCollection) =
    GetEnvVar "APPINSIGHTS_INSTRUMENTATIONKEY"
    |> Option.map services.AddApplicationInsightsTelemetry
    |> Option.defaultValue services

let app = application {
    url ("http://0.0.0.0:" + port.ToString() + "/")
    router WebServer.webApp
    memory_cache
    use_static publicPath
    service_config configureSerialization
    service_config configureAzure
    use_gzip
}

run app