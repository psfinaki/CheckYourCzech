module ApplicationInsightsLogger

open System
open Microsoft.ApplicationInsights
open Microsoft.ApplicationInsights.Extensibility

let getTelemetryClient() =
    let key = Environment.GetEnvironmentVariable "APPINSIGHTS_INSTRUMENTATIONKEY"
    TelemetryConfiguration.Active.InstrumentationKey <- key
    TelemetryClient()

let getLogger() = (lazy getTelemetryClient()).Value

let logTrace (message: string) =
    let logger = getLogger()
    logger.TrackTrace message
    logger.Flush()

let logError (error: exn, data) =
    let logger = getLogger()
    logger.TrackException(error, dict[data])
    logger.Flush()

