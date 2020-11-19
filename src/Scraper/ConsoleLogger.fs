module Scraper.ConsoleLogger

let logTrace (message: string) = printf $"{message}" 

let logError (error: exn) = printf $"{error}"
