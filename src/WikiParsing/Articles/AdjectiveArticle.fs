module WikiParsing.Articles.AdjectiveArticle

open Common.GrammarCategories.Adjectives
open Common.WikiArticles
open WikiParsing.AdjectiveWikiParsing
open WikiParsing.WikiString
open WikiParsing.Articles.Article
open WikiParsing.ConcreteArticles

let getNumberOfDeclensions (AdjectiveArticleWithPlural (AdjectiveArticle article)) = 
    article
    |> matches [
        Any
        Is "skloňování"
    ]
    |> Seq.length

let private hasRequiredInfoPlural (AdjectiveArticle article) = 
    article |> isMatch [
        Is "přídavné jméno"
        Is "skloňování"
    ]

let private hasRequiredInfoComparative (AdjectiveArticle article) =
    article |> isMatch [
        Is "přídavné jméno"
        Is "stupňování"
    ]

let private getWikiDeclension article = 
    let numberOfDeclensions = article |> getNumberOfDeclensions

    match numberOfDeclensions with
    | 1 -> getFirstDeclension article
    | 2 -> getSecondDeclension article
    | 3 -> getThirdDeclension article
    | _ -> invalidOp "Invalid article"

let getDeclension article : Declension =
    let declension = getWikiDeclension article
    {
        SingularNominative = declension.SingularNominative |> getForm
        SingularGenitive = declension.SingularGenitive
        SingularDative = declension.SingularDative |> getForm
        SingularAccusative = declension.SingularAccusative |> getForm
        SingularVocative = declension.SingularVocative |> getForm
        SingularLocative = declension.SingularLocative |> getForm
        SingularInstrumental = declension.SingularInstrumental |> getForm
        PluralNominative = declension.PluralNominative |> getForm
        PluralGenitive = declension.PluralGenitive |> getForm
        PluralDative = declension.PluralDative |> getForm
        PluralAccusative = declension.PluralAccusative |> getForm
        PluralVocative = declension.PluralVocative |> getForm
        PluralLocative = declension.PluralLocative |> getForm
        PluralInstrumental = declension.PluralInstrumental |> getForm
    }

let getComparison article =
    let comparison = getComparison article
    {
        Positive = comparison.Positive |> getForm
        Comparatives = comparison.Comparative |> getForms
    }

let parseAdjectivePlural article = 
    if article |> hasRequiredInfoPlural
    then Some (AdjectiveArticleWithPlural article)
    else None

let parseAdjectiveComparative article =
    if article |> hasRequiredInfoComparative
    then Some (AdjectiveArticleWithComparative article)
    else None

let parseAdjectiveArticle article =
    let (AdjectiveArticle { Title = title }) = article
    {
        CanonicalForm = title
        Declension =
            article
            |> parseAdjectivePlural
            |> Option.map getDeclension
        Comparison = 
            article
            |> parseAdjectiveComparative
            |> Option.map getComparison
    }
