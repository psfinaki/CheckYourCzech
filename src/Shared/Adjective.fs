module Adjective

open FSharp.Data
open Microsoft.WindowsAzure.Storage.Table
open Newtonsoft.Json
open Article

type Wiki = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">

let getComparatives word =
    let url = "https://cs.wiktionary.org/wiki/" + word
    let data = Wiki.Load url
    let answer = data.Tables.``Stupňování[editovat]``.Rows.[1].tvar
    answer.Split "/" |> Array.map (fun s -> s.Trim())

let isValid =
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "přídavné jméno")
    >> Option.bind (tryGetPart "stupňování")
    >> Option.isSome

type Adjective(word) =
    inherit TableEntity(word, word)
    
    new() = Adjective null

    member val Positive     = word |> Storage.mapSafe id                                             with get, set
    member val Comparatives = word |> Storage.mapSafe getComparatives |> JsonConvert.SerializeObject with get, set

let record word =
    if 
        isValid word
    then 
        word |> Adjective |> Storage.upsert "adjectives"
