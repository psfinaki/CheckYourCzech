module Plural

open Article
open Genders
open Microsoft.WindowsAzure.Storage.Table
open Noun

let getSingulars = getDeclension Case.Nominative Number.Singular
let getPlurals = getDeclension Case.Nominative Number.Plural

let isValid word = 
    let nounPart = ArticleParser.tryGetNoun word

    let hasDeclension =
        nounPart
        |> Option.bind (tryGetPart "skloňování")
        |> Option.isSome

    let hasGender = 
        nounPart
        |> Option.bind (tryGetInfo "rod")
        |> Option.bind tryTranslateGender
        |> Option.isSome

    let hasOneSingular = 
        getSingulars 
        >> Seq.hasOneElement

    let hasPlurals = 
        getPlurals
        >> Seq.any

    hasDeclension && 
    hasGender && 
    hasOneSingular word && 
    hasPlurals word

type Plural(word) =
    inherit TableEntity(word, word)
    
    new() = Plural null

    member val Singular = word |> Storage.mapSafeString id                 with get, set
    member val Gender   = word |> Storage.mapSafeObject (getGender >> box) with get, set
    member val Pattern  = word |> Storage.mapSafeStringOption getPattern   with get, set
    member val Plurals  = word |> Storage.mapSafeString getPlurals         with get, set

let record word =
    if 
        isValid word
    then
        word |> Plural |> Storage.upsert "plurals"
