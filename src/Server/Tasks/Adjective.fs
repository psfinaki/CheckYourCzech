module Server.Tasks.Adjective

open FSharp.Control.Tasks.V2
open Giraffe
open Microsoft.AspNetCore.Http

open Server.Tasks.Utils
open Storage.Storage
open Storage.ExerciseModels.AdjectivePlural
open Storage.ExerciseModels.AdjectiveComparative

let getAdjectivePluralsTask next (ctx : HttpContext) =
    task { 
        let! adjective = tryGetRandom<AdjectivePlural> "adjectiveplurals" []
    
        let getTask (adjective: AdjectivePlural) = 
            let singular = adjective.Singular
            let plural = adjective.Plural
            Task(singular, [| plural |])

        let task = adjective |> Option.map getTask |> Option.toObj 
        return! Successful.OK task next ctx
    }

let getAdjectiveComparativesTask next (ctx : HttpContext) =
    task { 
        let regularityFromQuery = ctx.GetQueryStringValue "isRegular"
        let regularityFilter = getAzureFilter "IsRegular" Bool regularityFromQuery
    
        let filters = [ regularityFilter ] |> Seq.choose id
        let! adjective = tryGetRandom<AdjectiveComparative> "adjectivecomparatives" filters
    
        let getTask (adjective: AdjectiveComparative) = 
            let positive = adjective.Positive
            let comparatives = adjective.Comparatives
            Task(positive, comparatives)
    
        let task = adjective |> Option.map getTask |> Option.toObj 
        return! Successful.OK task next ctx
    }
