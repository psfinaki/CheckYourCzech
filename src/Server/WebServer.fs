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
        
        let noun = tryGetRandom<NounPlural.Plural> "plurals" filters
        let getTask (noun: NounPlural.Plural) = 
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

        let noun = tryGetRandom<NounAccusative.Accusative> "accusatives" filters
        let getTask (noun: NounAccusative.Accusative) = 
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
        let adjective = tryGetRandom<AdjectiveComparative.Comparative> "comparatives" filters
        
        let getTask (adjective: AdjectiveComparative.Comparative) = 
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
            
        let verb = tryGetRandom<VerbImperative.Imperative> "imperatives" filters

        let getTask (verb: VerbImperative.Imperative) = 
            let indicative = getAs<string> verb.Indicative
            let imperatives = getAs<string []> verb.Imperatives
            Task(indicative, imperatives)

        let task = verb |> Option.map getTask |> Option.toObj 
        return! Successful.OK task next ctx
    }

let getParticiplesTask next (ctx: HttpContext) =
    task {
        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilter = getFilter "Pattern" String patternFromQuery

        let regularityFromQuery = ctx.GetQueryStringValue "isRegular"
        let regularityFilter = getFilter "IsRegular" Bool regularityFromQuery

        let filters =
            [ patternFilter; regularityFilter ] 
            |> Seq.choose id
        
        let verb = tryGetRandom<VerbParticiple.Participle> "participles" filters

        let getTask (verb: VerbParticiple.Participle) = 
            let infinitive = getAs<string> verb.Infinitive
            let participles = getAs<string []> verb.Participles
            Task(infinitive, participles)

        let task = verb |> Option.map getTask |> Option.toObj 
        return! Successful.OK task next ctx
    }

let webApp = router {
    get "/api/nouns/plurals"      getPluralsTask
    get "/api/nouns/accusatives"  getAccusativesTask
    get "/api/adjectives/comparatives" getComparativesTask
    get "/api/verbs/imperatives"  getImperativesTask
    get "/api/verbs/participles"  getParticiplesTask
}
