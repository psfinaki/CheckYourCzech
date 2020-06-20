module Scraper.WordRegistration.NounRegistration

open Scraper.ExerciseCreation.Nouns
open Storage.Storage
open Storage.ExerciseModels.Noun
open Common.WikiArticles
open WikiParsing.Articles.NounArticle

let registerNounDeclension = Noun >> upsert "nouns"

let registerNoun article = 
    let noun = parseNounArticle article

    let nounDeclension = 
        noun
        |> NounDeclension.Create noun.CanonicalForm
        |> Option.map registerNounDeclension

    [ nounDeclension ] |> List.choose id
