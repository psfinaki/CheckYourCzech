module Tasks.Verb

open FSharp.Control.Tasks.V2
open Giraffe
open Storage
open Microsoft.AspNetCore.Http
open Tasks.Utils

let getVerbImperativesTask next (ctx : HttpContext) =
    task {
        let classFromQuery = ctx.GetQueryStringValue "class"
        let classFilter = getAzureFilter "Class" Int classFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilter = getAzureFilter "Pattern" String patternFromQuery

        let filters = 
            [ classFilter; patternFilter ]
            |> Seq.choose id
            
        let verb = tryGetRandom<VerbImperative.VerbImperative> "verbimperatives" filters

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
        
        let verb = tryGetRandom<VerbParticiple.VerbParticiple> "verbparticiples" filters

        let getTask (verb: VerbParticiple.VerbParticiple) = 
            let infinitive = getAs<string> verb.Infinitive
            let participles = getAs<string []> verb.Participles
            Task(infinitive, participles)

        let task = verb |> Option.map getTask |> Option.toObj 
        return! Successful.OK task next ctx
    }