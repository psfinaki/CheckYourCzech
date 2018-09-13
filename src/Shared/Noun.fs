module Noun

open FSharp.Data
open Article
open Gender
open Microsoft.WindowsAzure.Storage.Table
open Newtonsoft.Json

type Wiki = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let getGender =
    Article.getContent
    >> getPart "čeština"
    >> getPart "podstatné jméno"
    >> getInfo "rod "
    >> Gender.FromString

let getPlurals word = 
    let url = "https://cs.wiktionary.org/wiki/" + word
    let data = Wiki.Load url
    let answer = data.Tables.``Skloňování[editovat]``.Rows.[0].plurál
    
    match answer with
    | "—" | "" -> [||]
    | _ -> answer.Split "/" |> Array.map (fun s -> s.Trim()) 

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

    member val Singular = word |> Storage.mapSafe id with get, set
    member val Gender   = word |> Storage.mapSafe (getGender >> Gender.ToString) with get, set
    member val Plurals  = word |> Storage.mapSafe getPlurals |> JsonConvert.SerializeObject with get, set

let record word =
    if 
        isValid word
    then
        word |> Noun |> Storage.upsert "nouns"
