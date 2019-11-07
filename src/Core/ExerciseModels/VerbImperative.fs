module VerbImperative

open Microsoft.WindowsAzure.Storage.Table
open VerbClasses
open Verb
open Storage

type VerbImperative(word) =
    inherit TableEntity(word, word)

    let getIndicative = map id serializeObject ""
    let getImperatives = map getImperatives serializeObject ""
    let getClass = map getClass serializeOption<VerbClass> ""
    let getPattern = map getImperativePattern serializeOption<string> ""

    new() = VerbImperative null

    member val Indicative = getIndicative word with get, set
    member val Imperatives = getImperatives word with get, set
    member val Class = getClass word with get, set
    member val Pattern = getPattern word with get, set
