module Verb

open Microsoft.WindowsAzure.Storage.Table

type Verb() =
    inherit TableEntity(null, null)
    
    member val Indicative  : string = null with get, set
    member val Imperatives : string = null with get, set
