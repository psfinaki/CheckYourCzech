module NounRegistration

open Storage
open Noun
open GrammarCategories
open WikiArticles
open Exercises

let registerNounPlural nounArticleWithPlural =
    let (NounArticleWithPlural nounArticle) = nounArticleWithPlural
    let (NounArticle { Title = word }) = nounArticle

    upsert "nounplurals" (NounPlural.NounPlural {
        Id = word
        Singular = word
        Plurals = nounArticle |> getDeclension Case.Nominative Number.Plural
        Gender = nounArticle |> getGender
        Patterns = nounArticle |> getPatterns
    })

let registerNounAccusative nounArticleWithAccusative =
    let (NounArticleWithAccusative nounArticle) = nounArticleWithAccusative
    let (NounArticle { Title = word }) = nounArticle

    upsert "nounaccusatives" (NounAccusative.NounAccusative {   
        Id = word
        Nominative = word
        Accusatives = nounArticle |> getDeclension Case.Accusative Number.Singular
        Gender = nounArticle |> getGender
        Patterns = nounArticle |> getPatterns 
    })
