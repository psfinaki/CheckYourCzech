module Scraper.WordRegistration.AdjectiveRegistration

open Common.Exercises
open Common.WikiArticles
open Scraper.ExerciseCreation.Adjectives
open Storage.Storage
open Storage.ExerciseModels.AdjectiveComparative
open Storage.ExerciseModels.AdjectivePlural
open WikiParsing.Articles.AdjectiveArticle

let private registerPlural id model = AdjectivePlural(id, model) |> upsert "adjectiveplurals"
let private registerComparative id model = AdjectiveComparative(id, model) |> upsert "adjectivecomparatives"

let registerAdjective article =
    let adjective = parseAdjectiveArticle article

    let pluralRegistration = 
        adjective.Declension 
        |> Option.map AdjectivePlural.Create
        |> Option.map (registerPlural adjective.CanonicalForm)

    let comparativeRegistration = 
        adjective.Comparison 
        |> Option.bind AdjectiveComparative.Create
        |> Option.map (registerComparative adjective.CanonicalForm)

    [ pluralRegistration; comparativeRegistration ] |> List.choose id
