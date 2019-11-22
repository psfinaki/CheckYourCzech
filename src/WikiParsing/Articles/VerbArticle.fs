module VerbArticle

open FSharp.Data
open WikiString
open GrammarCategories
open Conjugation
open Article

type WikiVerb = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">
type WikiParticiplesTable2 = HtmlProvider<"https://cs.wiktionary.org/wiki/musit">
type WikiParticiplesTable3 = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">

let getVerbProvider =
    getUrl
    >> WikiVerb.Load

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
    |> getChildPart "čeština"
    |> getChildPart "sloveso"
    |> getTables
    |> Seq.map fst
    |> Seq.findIndex ((=) "Příčestí")
    |> getParticipleByTableIndex word

let getParticiples = getWikiParticiples >> getForms
    
let getImperatives verb =
    let data = getVerbProvider verb
    let answer = data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - 2.``
    getForms answer
    
let getConjugations verb : ConjugationMapping = 
    let data = getVerbProvider verb
    let conjugationKeyValue num per = 
        let answer = 
            match num, per with
            | Singular, First  -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 1.``
            | Singular, Second -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 2.``
            | Singular, Third  -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 3.``
            | Plural,   First  -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo množné - 1.``
            | Plural,   Second -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo množné - 2.``
            | Plural,   Third  -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo množné - 3.``
    
        (num, per), getForms answer
    
    dict[
        conjugationKeyValue Singular First;
        conjugationKeyValue Singular Second;
        conjugationKeyValue Singular Third;
        conjugationKeyValue Plural First;
        conjugationKeyValue Plural Second;
        conjugationKeyValue Plural Third;
    ]

let getThirdPersonSingular verb = 
    let data = getVerbProvider verb
    let answer = data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 3.``
    getForms answer
