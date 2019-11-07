module AdjectivePlural

open Microsoft.WindowsAzure.Storage.Table
open Adjective
open Storage

type AdjectivePlural(word) =
    inherit TableEntity(word, word)
    
    let getSingular = map id serializeObject ""
    let getPlural = map getPlural serializeObject ""

    new() = AdjectivePlural null

    member val Singular = getSingular word with get, set
    member val Plural = getPlural word with get, set
