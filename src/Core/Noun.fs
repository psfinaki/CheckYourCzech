module Noun

open FSharp.Data
open Article
open Genders
open Microsoft.WindowsAzure.Storage.Table
open WikiString

type EditableArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">
type LockedArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/debil">

type Case = 
    | Nominative = 0
    | Genitive = 1
    | Dative = 2
    | Accusative = 3
    | Vocative = 4
    | Locative = 5
    | Instrumental = 6

type Number =
    | Singular
    | Plural

let getGender =
    getContent
    >> getPart "čeština"
    >> getPart "podstatné jméno"
    >> getInfo "rod "
    >> Gender.FromString

let isIndeclinable = 
    getContent
    >> getPart "čeština"
    >> getPart "podstatné jméno"
    >> getPart "skloňování"
    >> tryGetInfo "nesklonné"
    >> Option.isSome

let getUrl = (+) "https://cs.wiktionary.org/wiki/"

let getDeclensionEditable case number word =
    let data = word |> getUrl |> EditableArticle.Load
    match number with
    | Singular ->
        data.Tables.``Skloňování[editovat]``.Rows.[case].singulár
    | Plural -> 
        data.Tables.``Skloňování[editovat]``.Rows.[case].plurál

let getDeclensionLocked case number word =
    let data = word |> getUrl |> LockedArticle.Load
    match number with
    | Singular ->
        data.Tables.Skloňování.Rows.[case].singulár
    | Plural -> 
        data.Tables.Skloňování.Rows.[case].plurál

let getDeclension (case: Case) number word = 
    match word with
    | _ when word |> isIndeclinable ->
        word
    | _ when word |> isEditable ->
        getDeclensionEditable (int case) number word
    | _ when word |> isLocked ->
        getDeclensionLocked (int case) number word
    | word -> 
        invalidOp ("Odd word: " + word)

let getSingulars = getDeclension Case.Nominative Number.Singular >> getForms
let getPlurals = getDeclension Case.Nominative Number.Plural >> getForms
let getAccusatives = getDeclension Case.Nominative Number.Singular >> getForms

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
