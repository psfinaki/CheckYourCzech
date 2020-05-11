module AdjectiveRegistration

open Storage
open Adjective
open WikiArticles
open Exercises

let registerAdjectivePlural adjectiveArticleWithPlural =
    let (AdjectiveArticleWithPlural adjectiveArticle) = adjectiveArticleWithPlural
    let (AdjectiveArticle { Title = word }) = adjectiveArticle

    upsert "adjectiveplurals" (AdjectivePlural.AdjectivePlural {
        Id = word
        Singular = word
        Plural = adjectiveArticleWithPlural |> getPlural
    })

let registerAdjectiveComparative adjectiveArticleWithComparative =
    let (AdjectiveArticleWithComparative adjectiveArticle) = adjectiveArticleWithComparative
    let (AdjectiveArticle { Title = word }) = adjectiveArticle

    upsert "adjectivecomparatives" (AdjectiveComparative.AdjectiveComparative {
        Id = word
        Positive = word
        Comparatives = adjectiveArticleWithComparative |> getComparatives
        IsRegular = adjectiveArticleWithComparative |> hasRegularComparative
    })
