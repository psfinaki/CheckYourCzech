module Scraper.Logging

type LogMessage = 
    | Trace of string
    | Exception of exn * (string * string)
 
let consoleLogger = function
    | Trace m -> ConsoleLogger.log m
    | Exception (exn, _) -> ConsoleLogger.log exn

let aiLogger = function
    | Trace m -> ApplicationInsightsLogger.logTrace m
    | Exception (exn, data) -> ApplicationInsightsLogger.logError (exn, data)
