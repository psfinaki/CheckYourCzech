module VerbImperative

open Microsoft.WindowsAzure.Storage.Table
open Verb
open Storage

type VerbImperative(word) =
    inherit TableEntity(word, word)

    let getIndicative = map id serializeObject ""
    let getImperatives = map getImperatives serializeObject ""
    // Class cannot be int as with int the value will be 0 for verbs without class
    let getClass = map getClass serializeIntOption ""
    let getPattern = map getImperativePattern serializeStringOption ""

    new() = VerbImperative null

    member val Indicative = getIndicative word with get, set
    member val Imperatives = getImperatives word with get, set
    member val Class = getClass word with get, set
    member val Pattern = getPattern word with get, set
