module Core.Validation.NounValidation

open Core.Reflexives
open Core.Nouns.Nominalization
open Common.WikiArticles

let parseNoun article = 
    if article.Title |> (not << isNominalization) &&
       article.Title |> (not << isReflexive)
    
    then Some (NounArticle article)
    else None
