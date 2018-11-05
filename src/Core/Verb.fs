module Verb

open FSharp.Data
open Microsoft.WindowsAzure.Storage.Table
open Article
open StringHelpers

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

let getWikiImperatives word =
    let url = "https://cs.wiktionary.org/wiki/" + word
    let data = WikiImperativesPresent.Load url
    data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - 2.``

let parseImperatives =
    let processSpecialForms = function
        | s when s |> startsWith "(knižně) " -> 
            s |> remove "(knižně) " |> Some
        | s when s |> startsWith "(řidč.) " -> 
            s |> remove "(řidč.) " |> Some
        | s when s |> startsWith "(hovorově) " -> 
            None
        | s -> 
            Some s
    
    split [|'/'; ','|] 
    >> Array.map trim
    >> Array.map processSpecialForms
    >> Array.choose id

let getImperatives word =
    match hasImperatives word with
    | true -> 
        word
        |> getWikiImperatives
        |> parseImperatives
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