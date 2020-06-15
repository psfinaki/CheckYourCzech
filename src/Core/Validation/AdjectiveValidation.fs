module Core.Validation.AdjectiveValidation

open Core.Adjectives.Comparison
open Common.WikiArticles

let isValidAdjective = isPositive

let parseAdjective article = 
    if article.Title |> isValidAdjective
    then Some (AdjectiveArticle article)
    else None
