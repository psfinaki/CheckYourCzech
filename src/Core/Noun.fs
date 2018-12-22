module Noun

open FSharp.Data
open Article
open Gender
open Microsoft.WindowsAzure.Storage.Table
open WikiString

type CommonArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">
type LockedArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/debil">

let getGender =
    getContent
    >> getPart "čeština"
    >> getPart "podstatné jméno"
    >> getInfo "rod "
    >> Gender.FromString

let getWikiSingular word = 
    let url = "https://cs.wiktionary.org/wiki/" + word
    match isLocked word with
    | false ->
        let data = CommonArticle.Load url
        data.Tables.``Skloňování[editovat]``.Rows.[0].singulár
    | true ->
        let data = LockedArticle.Load url
        data.Tables.Skloňování.Rows.[0].singulár

let getWikiPlural word = 
    let url = "https://cs.wiktionary.org/wiki/" + word
    match isLocked word with
    | false ->
        let data = CommonArticle.Load url
        data.Tables.``Skloňování[editovat]``.Rows.[0].plurál
    | true ->
        let data = LockedArticle.Load url
        data.Tables.Skloňování.Rows.[0].plurál

let getWikiAccusative word = 
    let url = "https://cs.wiktionary.org/wiki/" + word
    match isLocked word with
    | false ->
        let data = CommonArticle.Load url
        data.Tables.``Skloňování[editovat]``.Rows.[3].singulár
    | true ->
        let data = LockedArticle.Load url
        data.Tables.Skloňování.Rows.[3].singulár

let getSingulars = getWikiSingular >> getForms
let getPlurals = getWikiPlural >> getForms
let getAccusatives = getWikiAccusative >> getForms

let isValid word = 
    let nounPart =
        word
        |> tryGetContent
        |> Option.bind (tryGetPart "čeština")
        |> Option.bind (tryGetPart "podstatné jméno")

    let hasDeclension =
        nounPart
        |> Option.bind (tryGetPart "skloňování")
        |> Option.isSome

    let hasGender = 
        nounPart
        |> Option.bind (tryGetInfo "rod")
        |> Option.bind Gender.TryFromString
        |> Option.isSome

    hasDeclension && hasGender

type Noun(word) =
    inherit TableEntity(word, word)
    
    new() = Noun null

    member val Singulars   = word |> Storage.mapSafeString getSingulars                   with get, set
    member val Gender      = word |> Storage.mapSafeString (getGender >> Gender.ToString) with get, set
    member val Plurals     = word |> Storage.mapSafeString getPlurals                     with get, set
    member val Accusatives = word |> Storage.mapSafeString getAccusatives                 with get, set

let record word =
    if 
        isValid word
    then
        word |> Noun |> Storage.upsert "nouns"
