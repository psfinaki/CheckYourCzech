module Server.Tasks.Verb

open FSharp.Control.Tasks.V2
open Giraffe
open Microsoft.AspNetCore.Http

open Common.Translations
open Common.Utils
open Common.GrammarCategories.Verbs
open Server.Tasks.Utils
open Storage.ExerciseModels
open Storage.Storage

let getVerbImperativesTask next (ctx : HttpContext) =
    task {
        let classFromQuery = ctx.GetQueryStringValue "class"
        let classFilter = getAzureFilter "Class" Int classFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilter = getAzureFilter "Pattern" String patternFromQuery

        let filters = 
            [ classFilter; patternFilter ]
            |> Seq.choose id
            
        let! verb = tryGetRandom<VerbImperative.VerbImperative> "verbimperatives" filters
        match verb with
        | Some verb ->
            let getTask (verb: VerbImperative.VerbImperative) = 
                let indicative = verb.Indicative
                let imperatives = verb.Imperatives
                { Word = indicative; Answers = imperatives |> Seq.toArray }

            let task = verb |> getTask
            return! Successful.OK task next ctx
        | None ->
            return! RequestErrors.NOT_FOUND "Not Found" next ctx
    }

let getVerbParticiplesTask next (ctx: HttpContext) =
    task {
        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilter = getAzureFilter "Pattern" String patternFromQuery

        let regularityFromQuery = ctx.GetQueryStringValue "isRegular"
        let regularityFilter = getAzureFilter "IsRegular" Bool regularityFromQuery

        let filters =
            [ patternFilter; regularityFilter ] 
            |> Seq.choose id
        
        let! verb = tryGetRandom<VerbParticiple.VerbParticiple> "verbparticiples" filters
        match verb with
        | Some verb ->
            let getTask (verb: VerbParticiple.VerbParticiple) = 
                let infinitive = verb.Infinitive
                let participles = verb.Participles
                { Word = infinitive; Answers = participles |> Seq.toArray }

            let task = verb |> getTask
            return! Successful.OK task next ctx
        | None ->
            return! RequestErrors.NOT_FOUND "Not Found" next ctx
    }

let getVerbConjugationTask next (ctx: HttpContext) =
    task {
        let classFromQuery = ctx.GetQueryStringValue "class"
        let classFilter = getAzureFilter "Class" Int classFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilter = getAzureFilter "Pattern" String patternFromQuery

        let filters = 
            [ classFilter; patternFilter ]
            |> Seq.choose id
        
        let getConjugation (conjugation: VerbConjugation.VerbConjugation) = function
            | FirstSingular  -> conjugation.FirstSingular
            | SecondSingular -> conjugation.SecondSingular
            | ThirdSingular  -> conjugation.ThirdSingular
            | FirstPlural    -> conjugation.FirstPlural
            | SecondPlural   -> conjugation.SecondPlural
            | ThirdPlural    -> conjugation.ThirdPlural

        let! verb = tryGetRandom<VerbConjugation.VerbConjugation> "verbconjugation" filters
        match verb with
        | Some verb ->
            let getTask (verb: VerbConjugation.VerbConjugation) =
                let pronoun = getRandomUnion<Pronoun>
                let infinitive = verb.Infinitive
                let answers = getConjugation verb pronoun
                let pn = pronounToString pronoun
                let word = $"({infinitive}) {pn} _____" 
                { Word = word; Answers = answers |> Seq.toArray }

            let task = verb |> getTask
            return! Successful.OK task next ctx
        | None ->
            return! RequestErrors.NOT_FOUND "Not Found" next ctx
    }
