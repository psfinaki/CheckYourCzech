module Scraper.WordRegistration.VerbRegistration

open Common.WikiArticles
open Scraper.ExerciseCreation.Verbs
open Storage.Storage
open Storage.ExerciseModels.VerbImperative
open Storage.ExerciseModels.VerbParticiple
open Storage.ExerciseModels.VerbConjugation
open WikiParsing.Articles.VerbArticle

let registerVerbConjugation id model = VerbConjugation(id, model) |> upsert "verbconjugation"
let registerVerbImperative id model = VerbImperative(id, model) |> upsert "verbimperatives"
let registerVerbParticiple id model = VerbParticiple(id, model) |> upsert "verbparticiples"

let registerVerb verbArticle =
    let verb = parseVerbArticle verbArticle

    let conjugationRegistration = 
        verb
        |> VerbConjugation.Create
        |> Option.map (registerVerbConjugation verb.CanonicalForm)

    let imperativeRegistration = 
        verb
        |> VerbImperative.Create
        |> Option.map (registerVerbImperative verb.CanonicalForm)

    let participleRegistration = 
        verb
        |> VerbParticiple.Create
        |> Option.map (registerVerbParticiple  verb.CanonicalForm)

    [ conjugationRegistration; imperativeRegistration; participleRegistration ] 
    |> List.choose id
