module Verb

open FSharp.Data
open Microsoft.WindowsAzure.Storage.Table
open Article

type Wiki = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">

let getImperatives word =
    let url = "https://cs.wiktionary.org/wiki/" + word
    let data = Wiki.Load url
    let answer = data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - 2.``
    answer.Split "/" |> Array.map (fun s -> s.Trim())

let isValid =
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "sloveso")
    >> Option.bind (tryGetPart "časování")
    >> Option.isSome

type Verb(word) =
    inherit TableEntity(word, word)
    
    new() = Verb null
    
    member val Indicative  = word |> Storage.mapSafe id             with get, set
    member val Imperatives = word |> Storage.mapSafe getImperatives with get, set

let record word =
    if 
        isValid word
    then 
        word |> Verb |> Storage.upsert "verbs"