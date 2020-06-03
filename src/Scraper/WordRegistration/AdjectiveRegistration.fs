module Scraper.WordRegistration.AdjectiveRegistration

open Common.Exercises
open Common.WikiArticles
open Scraper.ExerciseCreation
open Storage.Storage
open Storage.ExerciseModels.AdjectiveComparative
open Storage.ExerciseModels.AdjectivePlural
open WikiParsing.Articles.AdjectiveArticle

let private registerPlural = AdjectivePlural >> upsert "adjectiveplurals"
let private registerComparative = AdjectiveComparative >> upsert "adjectivecomparatives"

let registerAdjective article =
    let adjective = parseAdjectiveArticle article

    let pluralRegistration = 
        adjective.Declension 
        |> Option.map (AdjectivePlural.Create adjective.CanonicalForm)
        |> Option.map registerPlural

    let comparativeRegistration = 
        adjective.Comparison 
        |> Option.bind (AdjectiveComparative.Create adjective.CanonicalForm)
        |> Option.map registerComparative

    [ pluralRegistration; comparativeRegistration ] |> List.choose id
