module Scraper.Program

open System

open Logging

let getEnvironment = 
    Environment.GetEnvironmentVariable "ASPNETCORE_ENVIRONMENT"

let getLogger = function
    | "local" -> consoleLogger
    | "azure" -> aiLogger
    | env -> failwithf "Unknown environment: %s" env

[<EntryPoint>]
let main _ =
    getEnvironment
    |> getLogger
    |> App.run

    0
