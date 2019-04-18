module Imperative

open Microsoft.WindowsAzure.Storage.Table
open Article
open WikiString
open Verb
open VerbPatternDetector

let getImperatives verb =
    let data = getVerbProvider verb
    let answer = data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - 2.``
    getForms answer

let isVerbWithImperative =
    ArticleParser.tryGetVerb
    >> Option.bind (tryGetPart "časování")
    >> Option.map getTables
    >> Option.map (Seq.map fst)
    >> Option.map (Seq.contains "Rozkazovací způsob")
    >> Option.contains(true)

let isValid word = 
    let isNotArchaicVerb = not << isArchaic
    word |> isVerbWithImperative &&
    word |> isNotArchaicVerb

type Imperative(word) =
    inherit TableEntity(word, word)

    new() = Imperative null

    member val Indicative  = word |> Storage.mapSafeString id                with get, set
    member val Imperatives = word |> Storage.mapSafeString getImperatives    with get, set
    // Class cannot be int as with int the value will be 0 for verbs without class
    member val Class       = word |> Storage.mapSafeIntOption getClass       with get, set
    member val Pattern     = word |> Storage.mapSafeStringOption getPattern  with get, set

let record word =
    if 
        isValid word
    then 
        word |> Imperative |> Storage.upsert "imperatives"
