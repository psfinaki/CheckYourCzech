module Scraper.Program

open System

open Logging

let getEnvironment = 
    Environment.GetEnvironmentVariable "ASPNETCORE_ENVIRONMENT"

let getLogger = function
    | "local" -> consoleLogger
    | "azure" -> aiLogger
    | env -> failwith $"Unknown environment: {env}"

[<EntryPoint>]
let main _ =
    getEnvironment
    |> getLogger
    |> App.run

    0
