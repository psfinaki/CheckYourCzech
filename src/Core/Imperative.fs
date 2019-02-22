module Imperative

open FSharp.Data
open Microsoft.WindowsAzure.Storage.Table
open Article
open WikiString
open StringHelper

type WikiVerb = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">

let wikiUrl = "https://cs.wiktionary.org/wiki/"

let getWikiData =
    (+) wikiUrl
    >> WikiVerb.Load

let getImperatives verb =
    let data = getWikiData verb
    let answer = data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - 2.``
    getForms answer

let getThirdPersonSingular verb = 
    let data = getWikiData verb
    let answer = data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 3.``
    getForms answer

let getClass =
    getThirdPersonSingular
    >> Seq.tryExactlyOne
    >> Option.map Verb.removeReflexive
    >> Option.map Verb.getClassByThirdPersonSingular

let isVerbWithImperative =
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "sloveso")
    >> Option.bind (tryGetPart "časování")
    >> Option.map getTables
    >> Option.map (Seq.map fst)
    >> Option.map (Seq.contains "Rozkazovací způsob")
    >> Option.isSome

let isValid word = 
    let isNotArchaicVerb = not << Verb.isArchaic
    word |> isVerbWithImperative &&
    word |> isNotArchaicVerb

type Imperative(word) =
    inherit TableEntity(word, word)

    new() = Imperative null

    member val Indicative  = word |> Storage.mapSafeString id             with get, set
    member val Imperatives = word |> Storage.mapSafeString getImperatives with get, set
    member val Class       = word |> Storage.mapSafeIntOption getClass    with get, set

let record word =
    if 
        isValid word
    then 
        word |> Imperative |> Storage.upsert "imperatives"
