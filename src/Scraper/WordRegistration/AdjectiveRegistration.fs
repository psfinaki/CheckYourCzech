module Scraper.WordRegistration.AdjectiveRegistration

open Common.Exercises
open Common.WikiArticles
open Scraper.ExerciseCreation.Adjectives
open Storage.Storage
open Storage.ExerciseModels.AdjectiveDeclension
open Storage.ExerciseModels.AdjectiveComparative
open WikiParsing.Articles.AdjectiveArticle

let private registerDeclension id model = AdjectiveDeclension(id, model) |> upsert "adjectivedeclension"
let private registerComparative id model = AdjectiveComparative(id, model) |> upsert "adjectivecomparatives"

let registerAdjective article =
    let adjective = parseAdjectiveArticle article

    let declensionRegistration = 
        adjective 
        |> AdjectiveDeclension.Create
        |> Option.map (registerDeclension adjective.CanonicalForm)

    let comparativeRegistration = 
        adjective.Comparison 
        |> Option.bind AdjectiveComparative.Create
        |> Option.map (registerComparative adjective.CanonicalForm)

    [ declensionRegistration; comparativeRegistration ] |> List.choose id
