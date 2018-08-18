module Adjective

open Microsoft.WindowsAzure.Storage.Table

type Adjective() =
    inherit TableEntity(null, null)
    
    member val Positive     : string = null with get, set
    member val Comparatives : string = null with get, set
