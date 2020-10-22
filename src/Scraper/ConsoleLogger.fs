module Scraper.ConsoleLogger

let logTrace (message: string) = 
    message
    |> string 
    |> printfn "%s"

let logError (error: exn) = 
    error
    |> string 
    |> printfn "%s"
