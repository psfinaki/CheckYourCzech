module Scraper.WordRegistration.VerbRegistration

open Common.WikiArticles
open Scraper.ExerciseCreation.Verbs
open Storage.Storage
open Storage.ExerciseModels.VerbImperative
open Storage.ExerciseModels.VerbParticiple
open Storage.ExerciseModels.VerbConjugation
open WikiParsing.Articles.VerbArticle

let registerVerbConjugation = VerbConjugation >> upsert "verbconjugation"
let registerVerbImperative = VerbImperative >> upsert "verbimperatives"
let registerVerbParticiple = VerbParticiple >> upsert "verbparticiples"

let registerVerb verbArticle =
    let verb = parseVerbArticle verbArticle

    let conjugationRegistration = 
        verb
        |> VerbConjugation.Create verb.CanonicalForm
        |> Option.map registerVerbConjugation

    let imperativeRegistration = 
        verb
        |> VerbImperative.Create verb.CanonicalForm
        |> Option.map registerVerbImperative

    let participleRegistration = 
        verb
        |> VerbParticiple.Create verb.CanonicalForm
        |> Option.map registerVerbParticiple

    [ conjugationRegistration; imperativeRegistration; participleRegistration ] 
    |> List.choose id
