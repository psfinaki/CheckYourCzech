module VerbArticle

open FSharp.Data
open WikiString
open Conjugation
open Article
open WikiArticles

type WikiVerb = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">
type WikiParticiplesTable2 = HtmlProvider<"https://cs.wiktionary.org/wiki/musit">
type WikiParticiplesTable3 = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">

let getVerbProvider (VerbArticle { Text = text }) =
    text
    |> WikiVerb.Parse

let getParticiplesTable2 (VerbArticleWithParticiple (VerbArticle { Text = text })) = 
    text
    |> WikiParticiplesTable2.Parse
    |> fun data -> data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - mužský životný i neživotný``

let getParticiplesTable3 (VerbArticleWithParticiple (VerbArticle { Text = text })) =
    text
    |> WikiParticiplesTable3.Parse
    |> fun data -> data.Tables.``Časování[editovat]3``.Rows.[0].``Číslo jednotné - mužský životný i neživotný``

let getParticipleByTableIndex article n = 
    let participleExtraction = dict[ 
        1, getParticiplesTable2
        2, getParticiplesTable3 ]

    participleExtraction.Item n article

let getWikiParticiples verbParticipleArticle =
    let (VerbArticleWithParticiple (VerbArticle article)) = verbParticipleArticle

    article
    |> ``match`` [
        Is "sloveso"
    ]
    |> Option.map getTables
    |> Option.map (Seq.map fst)
    |> Option.map (Seq.findIndex ((=) "Příčestí"))
    |> Option.map (getParticipleByTableIndex verbParticipleArticle)

let getParticiples = getWikiParticiples >> Option.map getForms >> Option.defaultValue Array.empty

let getImperatives (VerbArticleWithImperative article) =
    article
    |> getVerbProvider
    |> fun data -> data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - 2.``
    |> getForms 

let getConjugation p (VerbArticleWithConjugation article) = 
    let data = getVerbProvider article
    let answer =
        match p with
        | FirstSingular  -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 1.``
        | SecondSingular -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 2.``
        | ThirdSingular  -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 3.``
        | FirstPlural    -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo množné - 1.``
        | SecondPlural   -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo množné - 2.``
        | ThirdPlural    -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo množné - 3.``

    getForms answer

let getThirdPersonSingular = 
    getVerbProvider
    >> fun data -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 3.``
    >> getForms
