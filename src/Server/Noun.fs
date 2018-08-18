module Noun

open Microsoft.WindowsAzure.Storage.Table

type Noun() = 
    inherit TableEntity(null, null)

    member val Singular : string = null with get, set
    member val Gender   : string = null with get, set
    member val Plurals  : string = null with get, set
     