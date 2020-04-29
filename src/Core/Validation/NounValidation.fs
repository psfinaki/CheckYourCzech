module NounValidation

open Reflexives
open GrammarCategories
open Article
open NounArticle
open GenderTranslations
open Nominalization
open Common.Utils
open WikiArticles

let hasSingleDeclension case number = 
    getDeclension case number
    >> Seq.hasOneElement

let hasDeclension case number = 
    getDeclension case number
    >> Seq.any

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
       article.Title |> (not << isReflexive) &&
       NounArticle article |> hasSingleDeclension Case.Nominative Number.Singular
    
    then Some (NounArticle article)
    else None

let parseNounPlural =
    parseNoun
    >> Option.filter (hasDeclension Case.Nominative Number.Plural)
    >> Option.map NounArticleWithPlural

let parseNounAccusative =
    parseNoun
    >> Option.filter (hasDeclension Case.Accusative Number.Singular)
    >> Option.map NounArticleWithAccusative
