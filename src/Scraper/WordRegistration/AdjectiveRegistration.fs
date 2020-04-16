module AdjectiveRegistration

open Storage
open Adjective
open WikiArticles

let registerAdjectivePlural adjectiveArticleWithPlural =
    let (AdjectiveArticleWithPlural adjectiveArticle) = adjectiveArticleWithPlural
    let (AdjectiveArticle { Title = word }) = adjectiveArticle

    let singular = word |> serializeObject
    let plural = adjectiveArticleWithPlural |> getPlural |> serializeObject

    AdjectivePlural.AdjectivePlural(word, singular, plural)
    |> upsert "adjectiveplurals"

let registerAdjectiveComparative adjectiveArticleWithComparative =
    let (AdjectiveArticleWithComparative adjectiveArticle) = adjectiveArticleWithComparative
    let (AdjectiveArticle { Title = word }) = adjectiveArticle

    let positive = word |> serializeObject
    let comparatives = adjectiveArticleWithComparative |> getComparatives |> serializeObject
    let isRegular = adjectiveArticleWithComparative |> hasRegularComparative

    AdjectiveComparative.AdjectiveComparative(word, positive, comparatives, isRegular)
    |> upsert "adjectivecomparatives"
