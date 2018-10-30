module Verb

open FSharp.Data
open Microsoft.WindowsAzure.Storage.Table
open Article

type WikiImperativesPresent = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">
type WikiImperativesAbsent  = HtmlProvider<"https://cs.wiktionary.org/wiki/musit">

let hasImperatives = 
    getContent
    >> getPart "čeština"
    >> getPart "sloveso"
    >> getPart "časování"
    >> getTables
    >> Seq.map fst
    >> Seq.contains "Rozkazovací způsob"

let getImperatives word =
    match hasImperatives word with
    | true -> 
        let url = "https://cs.wiktionary.org/wiki/" + word
        let data = WikiImperativesPresent.Load url
        let answer = data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - 2.``
        answer.Split "/" |> Array.map (fun s -> s.Trim())
    | false ->
        [||]

let getWikiParticiples word =
    let url = "https://cs.wiktionary.org/wiki/" + word
    match hasImperatives word with
    | true ->
        let data = WikiImperativesPresent.Load url
        data.Tables.``Časování[editovat]3``.Rows.[0].``Číslo jednotné - mužský životný i neživotný``
    | false ->
        let data = WikiImperativesAbsent.Load url
        data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - mužský životný i neživotný``

let parseParticiples (s: string) = 
    s.Split "/" 
    |> Array.map (fun s -> s.Trim())

let getParticiples = getWikiParticiples >> parseParticiples

let isValid =
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "sloveso")
    >> Option.bind (tryGetPart "časování")
    >> Option.isSome

type Verb(word) =
    inherit TableEntity(word, word)
    
    new() = Verb null
    
    member val Indicative  = word |> Storage.mapSafeString id             with get, set
    member val Imperatives = word |> Storage.mapSafeString getImperatives with get, set
    member val Participles = word |> Storage.mapSafeString getParticiples with get, set

let record word =
    if 
        isValid word
    then 
        word |> Verb |> Storage.upsert "verbs"