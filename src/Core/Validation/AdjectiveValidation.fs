module Core.Validation.AdjectiveValidation

open WikiParsing.Articles.Article
open WikiParsing.Articles.AdjectiveArticle
open Core.Adjectives.Comparison
open Core.Adjectives.ComparativeBuilder
open Common.WikiArticles

let hasMorphologicalComparatives = 
    getComparatives
    >> Seq.filter isMorphologicalComparison
    >> (not << Seq.isEmpty)

let hasRequiredInfoComparative =
    isMatch [
        Is "přídavné jméno"
        Is "stupňování"
    ]

let hasRequiredInfoPlural = 
    isMatch [
        Is "přídavné jméno"
        Is "skloňování"
    ]

let isValidAdjective = isPositive

let parseAdjective article = 
    if article.Title |> isValidAdjective
    then Some (AdjectiveArticle article)
    else None

let parseAdjectiveComparative =
    parseAdjective
    >> Option.filter (fun (AdjectiveArticle article) -> hasRequiredInfoComparative article)
    >> Option.filter (AdjectiveArticleWithComparative >> hasMorphologicalComparatives)
    >> Option.filter (fun (AdjectiveArticle { Title = title }) -> canBuildComparative title)
    >> Option.map AdjectiveArticleWithComparative

let parseAdjectivePlural =
    parseAdjective
    >> Option.filter (fun (AdjectiveArticle article) -> hasRequiredInfoPlural article)
    >> Option.map AdjectiveArticleWithPlural
