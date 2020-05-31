module Core.Validation.NounValidation

open Core.Reflexives
open Core.Nouns.Nominalization
open WikiParsing.Articles.Article
open Common.GenderTranslations
open Common.GrammarCategories
open Common.Utils
open Common.WikiArticles

let hasRequiredInfo article =
    article
    |> isMatch [
        Is "podstatné jméno"
        Starts "skloňování"
    ]

    &&

    article
    |> ``match`` [
        Is "podstatné jméno"
    ] 
    |> Option.exists (hasInfo (OneOf (getAllUnion<Gender> |> Seq.map toString)))

let parseNoun article = 
    if article |> hasRequiredInfo &&
       article.Title |> (not << isNominalization) &&
       article.Title |> (not << isReflexive)
    
    then Some (NounArticle article)
    else None
