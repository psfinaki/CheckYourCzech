module Tasks.Noun

open FSharp.Control.Tasks.V2
open Giraffe
open Storage
open Microsoft.AspNetCore.Http
open Tasks.Utils

let getNounPluralsTask next (ctx: HttpContext) =
    task {
        let genderFromQuery = ctx.GetQueryStringValue "gender"
        let genderFilter = getAzureFilter "Gender" String genderFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilterCondition (pattern: string) (noun: NounPlural.NounPlural) = noun.Patterns.Contains(pattern)
        let patternFilter = getPostFilter patternFilterCondition patternFromQuery

        let azureFilters = [ genderFilter ] |> Seq.choose id
        let postFilters = [ patternFilter ] |> Seq.choose id
        
        let noun = tryGetRandomWithFilters<NounPlural.NounPlural> "nounplurals" azureFilters postFilters
        let getTask (noun: NounPlural.NounPlural) = 
            let singular = getAs<string> noun.Singular
            let plurals = getAs<string []> noun.Plurals
            Task(singular, plurals)
        
        let task = noun |> Option.map getTask |> Option.toObj
        return! Successful.OK task next ctx
    }

let getNounAccusativesTask next (ctx : HttpContext) =
    task {
        let genderFromQuery = ctx.GetQueryStringValue "gender"
        let genderFilter = getAzureFilter "Gender" String genderFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilterCondition (pattern: string) (noun: NounAccusative.NounAccusative) = noun.Patterns.Contains(pattern)
        let patternFilter = getPostFilter patternFilterCondition patternFromQuery

        let azureFilters = [ genderFilter ] |> Seq.choose id
        let postFilters = [ patternFilter ] |> Seq.choose id

        let noun = tryGetRandomWithFilters<NounAccusative.NounAccusative> "nounaccusatives" azureFilters postFilters
        let getTask (noun: NounAccusative.NounAccusative) = 
            let singular = getAs<string> noun.Nominative 
            let accusatives = getAs<string []> noun.Accusatives
            Task(singular, accusatives)

        let task = noun |> Option.map getTask |> Option.toObj
        return! Successful.OK task next ctx
    }
