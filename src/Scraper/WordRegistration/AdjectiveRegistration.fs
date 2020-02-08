module AdjectiveRegistration

open Storage
open Adjective

let registerAdjectivePlural word =
    let singular = word |> map id serializeObject ""
    let plural = word |> map getPlural serializeObject ""

    AdjectivePlural.AdjectivePlural(word, singular, plural)
    |> upsert "adjectiveplurals"

let registerAdjectiveComparative word =
    let positive = word |> map id serializeObject ""
    let comparatives = word |> map getComparatives serializeObject ""
    let isRegular = word |> map hasRegularComparative id false

    AdjectiveComparative.AdjectiveComparative(word, positive, comparatives, isRegular)
    |> upsert "adjectivecomparatives"
