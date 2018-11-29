module Imperative

open FSharp.Data
open Microsoft.WindowsAzure.Storage.Table
open Article
open WikiString

type WikiVerb = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">

let getImperatives word =
    let url = "https://cs.wiktionary.org/wiki/" + word
    let data = WikiVerb.Load url
    let answer = data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - 2.``
    getForms answer

let isValid =
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "sloveso")
    >> Option.bind (tryGetPart "časování")
    >> Option.map getTables
    >> Option.map (Seq.map fst)
    >> Option.map (Seq.contains "Rozkazovací způsob")
    >> Option.isSome

type Imperative(word) =
    inherit TableEntity(word, word)

    new() = Imperative null

    member val Indicative  = word |> Storage.mapSafeString id             with get, set
    member val Imperatives = word |> Storage.mapSafeString getImperatives with get, set

let record word =
    if 
        isValid word
    then 
        word |> Imperative |> Storage.upsert "imperatives"
