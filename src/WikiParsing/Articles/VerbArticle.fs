module WikiParsing.Articles.VerbArticle

open FSharp.Data

open WikiParsing.WikiString
open Common.Conjugation
open Common.WikiArticles
open Article

type WikiVerb = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">
type WikiParticiplesTable2 = HtmlProvider<"https://cs.wiktionary.org/wiki/musit">
type WikiParticiplesTable3 = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">

type VerbArticleWithImperative = VerbArticleWithImperative of VerbArticle
type VerbArticleWithParticiple = VerbArticleWithParticiple of VerbArticle
type VerbArticleWithConjugation = VerbArticleWithConjugation of VerbArticle

let private hasRequiredInfoConjugation (VerbArticle article) = 
    article |> isMatch [
        Is "sloveso"
        Is "časování"
    ]

let private hasRequiredInfoImperative (VerbArticle article) = 
    article |> ``match`` [
        Is "sloveso"
        Is "časování"
    ]
    |> Option.map getTables
    |> Option.map (Seq.map fst)
    |> Option.map (Seq.contains "Rozkazovací způsob")
    |> Option.contains true

let private hasRequiredInfoParticiple (VerbArticle article) = 
    article |> isMatch [
        Is "sloveso"
        Is "časování"
    ]

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

let getParticiples = getWikiParticiples >> Option.map getForms >> Option.defaultValue Seq.empty

let getImperatives (VerbArticleWithImperative article) =
    article
    |> getVerbProvider
    |> fun data -> data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - 2.``
    |> getForms 

let getConjugation (VerbArticleWithConjugation article) = 
    let data = getVerbProvider article
    {
        FirstSingular  = data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 1.`` |> getForms
        SecondSingular = data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 2.`` |> getForms
        ThirdSingular  = data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 3.`` |> getForms
        FirstPlural    = data.Tables.``Časování[editovat]``.Rows.[0].``Číslo množné - 1.`` |> getForms
        SecondPlural   = data.Tables.``Časování[editovat]``.Rows.[0].``Číslo množné - 2.`` |> getForms
        ThirdPlural    = data.Tables.``Časování[editovat]``.Rows.[0].``Číslo množné - 3.`` |> getForms
    }

let getThirdPersonSingular = 
    getVerbProvider
    >> fun data -> data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 3.``
    >> getForms

let parseVerbConjugation article = 
    if article |> hasRequiredInfoConjugation
    then Some (VerbArticleWithConjugation article)
    else None

let parseVerbImperative article =
    if article |> hasRequiredInfoImperative
    then Some (VerbArticleWithImperative article)
    else None

let parseVerbParticiple article =
    if article |> hasRequiredInfoParticiple
    then Some (VerbArticleWithParticiple article)
    else None

let parseVerbArticle article = 
    let (VerbArticle { Title = title }) = article
    {
        CanonicalForm = title
        Conjugation =
            article
            |> parseVerbConjugation
            |> Option.map (fun article -> {
                Infinitive = title
                Conjugation = article |> getConjugation
            })
        Imperative = 
            article
            |> parseVerbImperative
            |> Option.map (fun article -> {
                Indicative = title
                Imperatives = article |> getImperatives
            })
        Participle =
            article
            |> parseVerbParticiple
            |> Option.map (fun article -> {
                Infinitive = title
                Participles = article |> getParticiples
            })
    }
