module Accusative

open Article
open Genders
open Microsoft.WindowsAzure.Storage.Table
open Noun

let getNominatives = getDeclension Case.Nominative Number.Singular
let getAccusatives = getDeclension Case.Accusative Number.Singular

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
        |> Option.bind tryTranslateGender
        |> Option.isSome

    let hasOneNominative = 
        getNominatives
        >> Seq.hasOneElement

    hasDeclension && 
    hasGender && 
    hasOneNominative word

type Accusative(word) =
    inherit TableEntity(word, word)
    
    new() = Accusative null

    member val Nominative  = word |> Storage.mapSafeString id                 with get, set
    member val Gender      = word |> Storage.mapSafeObject (getGender >> box) with get, set
    member val Pattern     = word |> Storage.mapSafeStringOption getPattern   with get, set
    member val Accusatives = word |> Storage.mapSafeString getAccusatives     with get, set

let record word =
    if 
        isValid word
    then
        word |> Accusative |> Storage.upsert "accusatives"
