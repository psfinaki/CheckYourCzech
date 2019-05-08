module VerbImperative

open Microsoft.WindowsAzure.Storage.Table

let isValid word = 
    word |> Word.isVerb &&
    word |> Verb.hasImperative &&
    word |> Verb.isModern

let getIndicative = Storage.mapSafeString id
let getImperatives = Storage.mapSafeString Verb.getImperatives
// Class cannot be int as with int the value will be 0 for verbs without class
let getClass = Storage.mapSafeIntOption Verb.getClass
let getPattern = Storage.mapSafeStringOption VerbPatternDetector.getPattern

type Imperative(word) =
    inherit TableEntity(word, word)

    new() = Imperative null

    member val Indicative = getIndicative word with get, set
    member val Imperatives = getImperatives word with get, set
    member val Class = getClass with get, set
    member val Pattern = getPattern word with get, set

let record word =
    if 
        isValid word
    then 
        word |> Imperative |> Storage.upsert "imperatives"
