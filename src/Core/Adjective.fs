module Adjective

open FSharp.Data
open Microsoft.WindowsAzure.Storage.Table
open Article
open Stem

type Wiki = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">

let buildTheoreticalComparative word = 
    let getStem (word: string) = word.TrimEnd('í', 'ý')
    
    let addSuffix = function
        | stem when stem |> endsHard -> stem + "ější"
        | stem when stem |> endsSoft -> stem + "ejší"
        | _ -> invalidArg word "odd adjective"

    word
    |> getStem
    |> alternate
    |> addSuffix

let getComparatives word =
    let url = "https://cs.wiktionary.org/wiki/" + word
    let data = Wiki.Load url
    let answer = data.Tables.``Stupňování[editovat]``.Rows.[1].tvar
    answer.Split "/" |> Array.map (fun s -> s.Trim())

let isRegular word =
    let theoretical = buildTheoreticalComparative word
    let practical = getComparatives word
    practical |> Array.contains theoretical

let isValid =
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "přídavné jméno")
    >> Option.bind (tryGetPart "stupňování")
    >> Option.isSome

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
