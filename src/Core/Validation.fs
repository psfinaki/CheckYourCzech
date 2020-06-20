module Core.Validation

open Common.WikiArticles
open Core.Adjectives.Comparison
open Core.Reflexives
open Core.Nouns.Nominalization
open Core.Verbs.Archaisms

let parseNoun article = 
    if article.Title |> (not << isNominalization) &&
       article.Title |> (not << isReflexive)
    
    then Some (NounArticle article)
    else None

let parseAdjective article = 
    if article.Title |> isPositive
    then Some (AdjectiveArticle article)
    else None

let parseVerb article =
    if article.Title |> isModern
    then Some (VerbArticle article)
    else None
