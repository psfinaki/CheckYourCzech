module Scraper.WordRegistration.NounRegistration

open Storage.Storage
open Storage.ExerciseModels.Noun
open Core.Nouns.Noun
open Common.WikiArticles
open Common.Exercises

let registerNoun nounArticle =
    let (NounArticle { Title = word }) = nounArticle

    upsert "nouns" (Noun {
        Id = word
        Gender = nounArticle |> getGender
        Patterns = nounArticle |> getPatterns
        Declension = nounArticle |> getDeclension
    })
