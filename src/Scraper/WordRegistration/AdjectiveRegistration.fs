module Scraper.WordRegistration.AdjectiveRegistration

open Core.Adjectives.Adjective
open Common.WikiArticles
open Common.Exercises
open Storage.Storage
open Storage.ExerciseModels.AdjectiveComparative
open Storage.ExerciseModels.AdjectivePlural

let registerAdjectivePlural adjectiveArticleWithPlural =
    let (AdjectiveArticleWithPlural adjectiveArticle) = adjectiveArticleWithPlural
    let (AdjectiveArticle { Title = word }) = adjectiveArticle

    upsert "adjectiveplurals" (AdjectivePlural {
        Id = word
        Singular = word
        Plural = adjectiveArticleWithPlural |> getPlural
    })

let registerAdjectiveComparative adjectiveArticleWithComparative =
    let (AdjectiveArticleWithComparative adjectiveArticle) = adjectiveArticleWithComparative
    let (AdjectiveArticle { Title = word }) = adjectiveArticle

    upsert "adjectivecomparatives" (AdjectiveComparative {
        Id = word
        Positive = word
        Comparatives = adjectiveArticleWithComparative |> getComparatives
        IsRegular = adjectiveArticleWithComparative |> hasRegularComparative
    })
