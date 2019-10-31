module AdjectiveComparative

open Microsoft.WindowsAzure.Storage.Table
open Adjective
open Storage

type AdjectiveComparative(word) =
    inherit TableEntity(word, word)
    
    let getPositive = map id serializeObject ""
    let getComparatives = map getComparatives serializeObject ""
    let getIsRegular = map hasRegularComparative id false

    new() = AdjectiveComparative null

    member val Positive = getPositive word with get, set
    member val Comparatives = getComparatives word with get, set
    member val IsRegular = getIsRegular word with get, set
