module Tasks.Adjective

open FSharp.Control.Tasks.V2
open Giraffe
open Storage
open Microsoft.AspNetCore.Http
open Tasks.Utils

let getAdjectivePluralsTask next (ctx : HttpContext) =
    task { 
        let! adjective = tryGetRandom<AdjectivePlural.AdjectivePlural> "adjectiveplurals" []
    
        let getTask (adjective: AdjectivePlural.AdjectivePlural) = 
            let singular = getAs<string> adjective.Singular
            let plural = getAs<string> adjective.Plural
            Task(singular, [| plural |])

        let task = adjective |> Option.map getTask |> Option.toObj 
        return! Successful.OK task next ctx
    }

let getAdjectiveComparativesTask next (ctx : HttpContext) =
    task { 
        let regularityFromQuery = ctx.GetQueryStringValue "isRegular"
        let regularityFilter = getAzureFilter "IsRegular" Bool regularityFromQuery
    
        let filters = [ regularityFilter ] |> Seq.choose id
        let! adjective = tryGetRandom<AdjectiveComparative.AdjectiveComparative> "adjectivecomparatives" filters
    
        let getTask (adjective: AdjectiveComparative.AdjectiveComparative) = 
            let positive = getAs<string> adjective.Positive
            let comparatives = getAs<string []> adjective.Comparatives
            Task(positive, comparatives)
    
        let task = adjective |> Option.map getTask |> Option.toObj 
        return! Successful.OK task next ctx
    }
