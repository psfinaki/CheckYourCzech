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

let getParticipleByTableIndex article = function
    | 1 -> getParticipleWhenImperativeAbsent article
    | 2 -> getParticipleWhenImperativePresent article
    | _ -> invalidOp "Invalid article"

let getWikiParticiples verbParticipleArticle =
    let (VerbArticleWithParticiple (VerbArticle article)) = verbParticipleArticle

    article
    |> ``match`` [
        Is "sloveso"
    ]
    |> Option.get // we know it's there due to the article type
    |> getTables
    |> Seq.map fst
    |> Seq.findIndex ((=) "Příčestí")
    |> getParticipleByTableIndex verbParticipleArticle

let getParticiple verbArticle =
    let (VerbArticleWithParticiple (VerbArticle { Title = title })) = verbArticle
    let wikiParticiple = verbArticle |> getWikiParticiples
    {
        Infinitive = title
        Participles = wikiParticiple.SingularMasculineAnimate |> getForms
    }

let getImperative verbArticle =
    let (VerbArticleWithImperative (VerbArticle { Title = title })) = verbArticle
    let wikiImperative = getImperative verbArticle
    {
        Indicative = title
        Imperatives = wikiImperative.Singular |> getForms
    }

let getConjugation verbArticle = 
    let (VerbArticleWithConjugation (VerbArticle { Title = title })) = verbArticle
    let wikiConjugation = getConjugation verbArticle
    {
        Infinitive = title
        FirstSingular  = wikiConjugation.FirstSingular |> getForms
        SecondSingular = wikiConjugation.SecondSingular |> getForms
        ThirdSingular  = wikiConjugation.ThirdSingular |> getForms
        FirstPlural    = wikiConjugation.FirstPlural |> getForms
        SecondPlural   = wikiConjugation.SecondPlural |> getForms
        ThirdPlural    = wikiConjugation.ThirdPlural |> getForms
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
            |> Option.map getConjugation
        Imperative = 
            article
            |> parseVerbImperative
            |> Option.map getImperative
        Participle =
            article
            |> parseVerbParticiple
            |> Option.map getParticiple
    }
