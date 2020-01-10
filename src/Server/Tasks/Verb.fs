module Tasks.Verb

open FSharp.Control.Tasks.V2
open Giraffe
open Storage
open Microsoft.AspNetCore.Http
open Tasks.Utils
open Conjugation
open Verb

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

        let getTask (verb: VerbImperative.VerbImperative) = 
            let indicative = getAs<string> verb.Indicative
            let imperatives = getAs<string []> verb.Imperatives
            Task(indicative, imperatives)

        let task = verb |> Option.map getTask |> Option.toObj 
        return! Successful.OK task next ctx
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

        let getTask (verb: VerbParticiple.VerbParticiple) = 
            let infinitive = getAs<string> verb.Infinitive
            let participles = getAs<string []> verb.Participles
            Task(infinitive, participles)

        let task = verb |> Option.map getTask |> Option.toObj 
        return! Successful.OK task next ctx
    }

let getVerbConjugationTask next (ctx: HttpContext) =
    task {
        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilter = getAzureFilter "Pattern" String patternFromQuery

        let filters =
            [ patternFilter ] 
            |> Seq.choose id
        
        let! verb = tryGetRandom<VerbConjugation.VerbConjugation> "verbconjugation" filters
        let getTask (verb: VerbConjugation.VerbConjugation) =
            let pronoun = getRandomPronoun()
            let infinitive = getAs<string> verb.Infinitive
            let answers = getAs<string[]> (verb.Conjugation pronoun)
            let pn = pronounToString pronoun
            let word = sprintf "(%s) %s _____" infinitive pn 
            Task(word, answers)

        let task = verb |> Option.map getTask |> Option.toObj 
        return! Successful.OK task next ctx
    }
