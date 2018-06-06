﻿module Program

open System
open System.IO
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Newtonsoft.Json
open Giraffe
open Giraffe.Serialization.Json

let GetEnvVar var =
    match Environment.GetEnvironmentVariable(var) with
    | null -> None
    | value -> Some value

let getPortsOrDefault defaultVal =
    match Environment.GetEnvironmentVariable("SUAVE_FABLE_PORT") with
    | null -> defaultVal
    | value -> value |> uint16

let configureApp root (app : IApplicationBuilder) =
    app.UseStaticFiles()
       .UseGiraffe (WebServer.webApp root)

let configureServices (services : IServiceCollection) =
    // Add default Giraffe dependencies
    services.AddGiraffe() |> ignore

    // Configure JsonSerializer to use Fable.JsonConverter
    let fableJsonSettings = JsonSerializerSettings()
    fableJsonSettings.Converters.Add(Fable.JsonConverter())

    services.AddSingleton<IJsonSerializer>(
        NewtonsoftJsonSerializer(fableJsonSettings)) |> ignore

let configureLogging (loggerBuilder : ILoggingBuilder) =
    loggerBuilder.AddFilter(fun lvl -> lvl.Equals LogLevel.Error)
                 .AddConsole()
                 .AddDebug() |> ignore

[<EntryPoint>]
let main args =
    try
        let args = Array.toList args
        let clientPath =
            match args with
            | clientPath:: _  when Directory.Exists clientPath -> clientPath
            | _ ->
                // did we start from server folder?
                let devPath = Path.Combine("..","Client")
                if Directory.Exists devPath then devPath
                else
                    // maybe we are in root of project?
                    let devPath = Path.Combine("src","Client")
                    if Directory.Exists devPath then devPath
                    else @"./client"
            |> Path.GetFullPath

        let port = getPortsOrDefault 8085us

        WebHost
            .CreateDefaultBuilder()
            .UseWebRoot(clientPath)
            .UseContentRoot(clientPath)
            .ConfigureLogging(configureLogging)
            .ConfigureServices(configureServices)
            .Configure(Action<IApplicationBuilder> (configureApp clientPath))
            .UseUrls("http://0.0.0.0:" + port.ToString() + "/")
            .Build()
            .Run()
        0
    with
    | exn ->
        let color = Console.ForegroundColor
        Console.ForegroundColor <- System.ConsoleColor.Red
        Console.WriteLine(exn.Message)
        Console.ForegroundColor <- color
        1
