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
        let patternFilterCondition (pattern: string) (noun: Noun.Noun) = noun.Patterns |> Seq.contains pattern
        let patternFilter = getPostFilter patternFilterCondition patternFromQuery

        let caseFilter (noun: Noun.Noun) = 
            noun.SingularNominative |> Seq.any &&
            noun.PluralNominative |> Seq.any

        let azureFilters = [ genderFilter ] |> Seq.choose id
        let postFilters = [ patternFilter; Some caseFilter ] |> Seq.choose id
        
        let! noun = tryGetRandomWithFilters<Noun.Noun> "nouns" azureFilters postFilters
        let getTask (noun: Noun.Noun) = 
            let singular = noun.SingularNominative |> Seq.random
            let plurals = noun.PluralNominative
            Task(singular, plurals)
        
        let task = noun |> Option.map getTask |> Option.toObj
        return! Successful.OK task next ctx
    }

let getNounAccusativesTask next (ctx : HttpContext) =
    task {
        let genderFromQuery = ctx.GetQueryStringValue "gender"
        let genderFilter = getAzureFilter "Gender" String genderFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilterCondition (pattern: string) (noun: Noun.Noun) = noun.Patterns |> Seq.contains pattern
        let patternFilter = getPostFilter patternFilterCondition patternFromQuery

        let caseFilter (noun: Noun.Noun) = 
            noun.SingularNominative |> Seq.any &&
            noun.SingularAccusative |> Seq.any

        let azureFilters = [ genderFilter ] |> Seq.choose id
        let postFilters = [ patternFilter; Some caseFilter ] |> Seq.choose id

        let! noun = tryGetRandomWithFilters<Noun.Noun> "nouns" azureFilters postFilters
        let getTask (noun: Noun.Noun) = 
            let singular = noun.SingularNominative |> Seq.random
            let accusatives = noun.SingularAccusative
            Task(singular, accusatives)

        let task = noun |> Option.map getTask |> Option.toObj
        return! Successful.OK task next ctx
    }
