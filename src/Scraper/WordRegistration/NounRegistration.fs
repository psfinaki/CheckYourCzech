module Scraper.WordRegistration.NounRegistration

open Scraper.ExerciseCreation.Nouns
open Storage.Storage
open Storage.ExerciseModels.Noun
open Common.Exercises
open Common.WikiArticles
open WikiParsing.Articles.NounArticle

let registerNounDeclension id model = Noun(id, model) |> upsert "nouns"

let registerNoun article = 
    let noun = parseNounArticle article

    let nounDeclension = 
        noun
        |> Noun.Create
        |> Option.map (registerNounDeclension noun.CanonicalForm)

    [ nounDeclension ] |> List.choose id
