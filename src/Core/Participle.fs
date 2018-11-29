module Participle

open FSharp.Data
open Microsoft.WindowsAzure.Storage.Table
open Article
open WikiString

type WikiParticiplesTable2 = HtmlProvider<"https://cs.wiktionary.org/wiki/musit">
type WikiParticiplesTable3 = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">

let getParticiplesTable2 = 
    (+) "https://cs.wiktionary.org/wiki/"
    >> WikiParticiplesTable2.Load
    >> fun data -> data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - mužský životný i neživotný``

let getParticiplesTable3 =
    (+) "https://cs.wiktionary.org/wiki/"
    >> WikiParticiplesTable3.Load
    >> fun data -> data.Tables.``Časování[editovat]3``.Rows.[0].``Číslo jednotné - mužský životný i neživotný``

let getParticipleByTableIndex word n = 
    let participleExtraction = dict[ 
        1, getParticiplesTable2
        2, getParticiplesTable3 ]

    participleExtraction.Item n word

let getWikiParticiples word =
    word
    |> getContent
    |> getTables
    |> Seq.map fst
    |> Seq.findIndex ((=) "Příčestí")
    |> getParticipleByTableIndex word
        
let getParticiples = getWikiParticiples >> getForms

let isValid =
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "sloveso")
    >> Option.bind (tryGetPart "časování")
    >> Option.isSome

type Participle(word) =
    inherit TableEntity(word, word)

    new() =  Participle null

    member val Indicative  = word |> Storage.mapSafeString id             with get, set
    member val Participles = word |> Storage.mapSafeString getParticiples with get, set

let record word =
    if 
        isValid word
    then 
        word |> Participle |> Storage.upsert "participles"