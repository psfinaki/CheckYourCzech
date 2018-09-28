module Logger

open System
open Microsoft.ApplicationInsights
open Microsoft.ApplicationInsights.Extensibility

let getTelemetryClient() =
    let key = Environment.GetEnvironmentVariable "APPINSIGHTS_INSTRUMENTATIONKEY"
    TelemetryConfiguration.Active.InstrumentationKey <- key
    TelemetryClient()

let getLogger() = (lazy getTelemetryClient()).Value

let logMessage (message : string) =
    let logger = getLogger()
    logger.TrackTrace message
    logger.Flush()

let logError (error : exn) =
    let logger = getLogger()
    logger.TrackException error
    logger.Flush()
