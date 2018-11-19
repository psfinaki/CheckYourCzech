module Adjective

open FSharp.Data
open Microsoft.WindowsAzure.Storage.Table
open Article
open Stem
open WikiString

type Wiki = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">

let getStem (word: string) = word.TrimEnd('í', 'ý')

let buildTheoreticalComparative word = 
    let isComparativePossible = not <| (getStem word).EndsWith "c"

    let addSuffix = function
        | stem when stem |> endsHard -> stem + "ější"
        | stem when stem |> endsSoft -> stem + "ejší"
        | _ -> invalidArg word "odd adjective"

    if isComparativePossible
    then 
        word
        |> getStem
        |> alternate
        |> addSuffix
        |> Some
    else 
        None

let getPositive word =
    let url = "https://cs.wiktionary.org/wiki/" + word
    let data = Wiki.Load url
    let answer = data.Tables.``Stupňování[editovat]``.Rows.[0].tvar
    answer

let isSyntacticComparison (comparison: string) = comparison.StartsWith "více "

let getComparatives word =
    let isMorphologicalComparison = not << isSyntacticComparison
    
    let url = "https://cs.wiktionary.org/wiki/" + word
    let data = Wiki.Load url
    let wikiString = data.Tables.``Stupňování[editovat]``.Rows.[1].tvar

    wikiString 
    |> getForms
    |> Array.filter isMorphologicalComparison

let isRegular word =
    let theoretical = buildTheoreticalComparative word
    let practical = getComparatives word

    match theoretical with
    | Some option -> practical |> Array.contains option
    | None        -> practical |> Array.isEmpty

let hasAdjectiveContent = 
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "přídavné jméno")
    >> Option.bind (tryGetPart "stupňování")
    >> Option.isSome

let isValid word = 
    if word |> hasAdjectiveContent
    then
        let adjective = word
        let isPositive = adjective = getPositive adjective
        isPositive
    else
        false

type Adjective(word) =
    inherit TableEntity(word, word)
    
    new() = Adjective null

    member val Positive     = word |> Storage.mapSafeString id              with get, set
    member val Comparatives = word |> Storage.mapSafeString getComparatives with get, set
    member val IsRegular    = word |> Storage.mapSafeBool   isRegular       with get, set

let record word =
    if 
        isValid word
    then 
        word |> Adjective |> Storage.upsert "adjectives"
