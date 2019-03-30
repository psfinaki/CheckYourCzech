module WebServer

open FSharp.Control.Tasks.V2
open Giraffe
open Saturn
open Storage
open Microsoft.AspNetCore.Http

[<AllowNullLiteral>]
type Task(word, answers) = 
    member this.Word = word
    member this.Answers = answers

let getFilter columnName queryCondition = function
    | Ok parameterValue -> Some (columnName, queryCondition, box parameterValue)
    | Error _ -> None

let getPluralsTask next (ctx: HttpContext) =
    task {
        let genderFromQuery = ctx.GetQueryStringValue "gender"
        let genderFilter = getFilter "Gender" String genderFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilter = getFilter "Pattern" String patternFromQuery

        let filters = 
            [ genderFilter; patternFilter ] 
            |> Seq.choose id
        
        let noun = tryGetRandom<Plural.Plural> "plurals" filters
        let getTask (noun: Plural.Plural) = 
            let singular = getAs<string> noun.Singular
            let plurals = getAs<string []> noun.Plurals
            Task(singular, plurals)
        
        let task = noun |> Option.map getTask |> Option.toObj
        return! Successful.OK task next ctx
    }

let getAccusativesTask next (ctx : HttpContext) =
    task {
        let genderFromQuery = ctx.GetQueryStringValue "gender"
        let genderFilter = getFilter "Gender" String genderFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilter = getFilter "Pattern" String patternFromQuery

        let filters = 
            [ genderFilter; patternFilter ] 
            |> Seq.choose id

        let noun = tryGetRandom<Accusative.Accusative> "accusatives" filters
        let getTask (noun: Accusative.Accusative) = 
            let singular = getAs<string> noun.Nominative 
            let accusatives = getAs<string []> noun.Accusatives
            Task(singular, accusatives)

        let task = noun |> Option.map getTask |> Option.toObj
        return! Successful.OK task next ctx
    }

let getComparativesTask next (ctx : HttpContext) =
    task { 
        let regularityFromQuery = ctx.GetQueryStringValue "isRegular"
        let regularityFilter = getFilter "IsRegular" Bool regularityFromQuery

        let filters = [ regularityFilter ] |> Seq.choose id
        let adjective = tryGetRandom<Adjective.Adjective> "adjectives" filters
        
        let getTask (adjective: Adjective.Adjective) = 
            let positive = getAs<string> adjective.Positive
            let comparatives = getAs<string []> adjective.Comparatives
            Task(positive, comparatives)

        let task = adjective |> Option.map getTask |> Option.toObj 
        return! Successful.OK task next ctx
    }

let getImperativesTask next (ctx : HttpContext) =
    task {
        let classFromQuery = ctx.GetQueryStringValue "class"
        let classFilter = getFilter "Class" Int classFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilter = getFilter "Pattern" String patternFromQuery

        let filters = 
            [ classFilter; patternFilter ]
            |> Seq.choose id
            
        let verb = tryGetRandom<Imperative.Imperative> "imperatives" filters

        let getTask (verb: Imperative.Imperative) = 
            let indicative = getAs<string> verb.Indicative
            let imperatives = getAs<string []> verb.Imperatives
            Task(indicative, imperatives)

        let task = verb |> Option.map getTask |> Option.toObj 
        return! Successful.OK task next ctx
    }

let getParticiplesTask next (ctx: HttpContext) =
    task {
        let regularityFromQuery = ctx.GetQueryStringValue "isRegular"
        let regularityFilter = getFilter "IsRegular" Bool regularityFromQuery

        let filters = [ regularityFilter ] |> Seq.choose id
        let verb = tryGetRandom<Participle.Participle> "participles" filters

        let getTask (verb: Participle.Participle) = 
            let infinitive = getAs<string> verb.Infinitive
            let participles = getAs<string []> verb.Participles
            Task(infinitive, participles)

        let task = verb |> Option.map getTask |> Option.toObj 
        return! Successful.OK task next ctx
    }

let webApp = router {
    get "/api/plurals"      getPluralsTask
    get "/api/accusatives"  getAccusativesTask
    get "/api/comparatives" getComparativesTask
    get "/api/imperatives"  getImperativesTask
    get "/api/participles"  getParticiplesTask
}
