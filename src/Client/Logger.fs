module Logger

open Fable.Core.JsInterop
open Fable.Import.Browser

let setup() = 
    // this is okay: https://docs.microsoft.com/en-us/azure/azure-monitor/app/troubleshoot-faq#my-instrumentation-key-is-visible-in-my-web-page-source
    let key = "abd7a36c-47ae-4d13-8ead-ebefcb3556e1"
    let config = createObj [ "instrumentationKey" ==> key ]
    window?appInsights?downloadAndSetup config

let log message = window?appInsights?trackTrace message
