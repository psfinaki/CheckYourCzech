module VerbArticle

open FSharp.Data
open WikiString
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
    |> ``match`` [
        Is "sloveso"
    ]
    |> Option.map getTables
    |> Option.map (Seq.map fst)
    |> Option.map (Seq.findIndex ((=) "Příčestí"))
    |> Option.map (getParticipleByTableIndex word)
    // TODO:
    // There is a problem here: we couple this place
    // with the fact that this is called after the verb validation
    // where we check the existance of this table in the article.
    // So logically we know that the table exists
    // but code-wise we don't. This should be fixed.
    |> Option.defaultWith (fun () -> failwith "odd word")

let getParticiples = getWikiParticiples >> getForms
    
let getImperatives verb =
    let data = getVerbProvider verb
    let answer = data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - 2.``
    getForms answer

let getConjugation p verb = 
    let data = getVerbProvider verb
    let answer = 
        match p with
        | FirstSingular  -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 1.``
        | SecondSingular -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 2.``
        | ThirdSingular  -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 3.``
        | FirstPlural    -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo množné - 1.``
        | SecondPlural   -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo množné - 2.``
        | ThirdPlural    -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo množné - 3.``

    getForms answer

let getThirdPersonSingular verb = 
    let data = getVerbProvider verb
    let answer = data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 3.``
    getForms answer
