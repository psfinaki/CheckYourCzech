module WikiParsing.Articles.VerbArticle

open WikiParsing.ConcreteArticles
open WikiParsing.Raw.Verb
open WikiParsing.WikiString
open Common.GrammarCategories.Verbs
open Common.WikiArticles
open Article

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

let getParticipleByTableIndex article participleTableIndex = 
    let wikiParticiple = 
        match participleTableIndex with
        | 1 -> getParticipleWhenImperativeAbsent article
        | 2 -> getParticipleWhenImperativePresent article
        | _ -> invalidOp "Invalid article"

    wikiParticiple.SingularMasculineAnimate |> getForm

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

let getImperatives article =
    let wikiImperative = article |> getImperative
    wikiImperative.Singular |> getForms

let getConjugation article = 
    let conjugation = getConjugation article
    {
        FirstSingular  = conjugation.FirstSingular |> getForms
        SecondSingular = conjugation.SecondSingular |> getForms
        ThirdSingular  = conjugation.ThirdSingular |> getForms
        FirstPlural    = conjugation.FirstPlural |> getForms
        SecondPlural   = conjugation.SecondPlural |> getForms
        ThirdPlural    = conjugation.ThirdPlural |> getForms
    }

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
