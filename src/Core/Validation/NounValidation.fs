module NounValidation

open Reflexives
open GrammarCategories
open Article
open GenderTranslations
open Nominalization
open Common.Utils
open WikiArticles

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
