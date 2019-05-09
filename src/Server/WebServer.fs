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

let getNounPluralsTask next (ctx: HttpContext) =
    task {
        let genderFromQuery = ctx.GetQueryStringValue "gender"
        let genderFilter = getFilter "Gender" String genderFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilter = getFilter "Pattern" String patternFromQuery

        let filters = 
            [ genderFilter; patternFilter ] 
            |> Seq.choose id
        
        let noun = tryGetRandom<NounPlural.NounPlural> "nounplurals" filters
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
        let genderFilter = getFilter "Gender" String genderFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilter = getFilter "Pattern" String patternFromQuery

        let filters = 
            [ genderFilter; patternFilter ] 
            |> Seq.choose id

        let noun = tryGetRandom<NounAccusative.NounAccusative> "nounaccusatives" filters
        let getTask (noun: NounAccusative.NounAccusative) = 
            let singular = getAs<string> noun.Nominative 
            let accusatives = getAs<string []> noun.Accusatives
            Task(singular, accusatives)

        let task = noun |> Option.map getTask |> Option.toObj
        return! Successful.OK task next ctx
    }

let getAdjectiveComparativesTask next (ctx : HttpContext) =
    task { 
        let regularityFromQuery = ctx.GetQueryStringValue "isRegular"
        let regularityFilter = getFilter "IsRegular" Bool regularityFromQuery

        let filters = [ regularityFilter ] |> Seq.choose id
        let adjective = tryGetRandom<AdjectiveComparative.AdjectiveComparative> "adjectivecomparatives" filters
        
        let getTask (adjective: AdjectiveComparative.AdjectiveComparative) = 
            let positive = getAs<string> adjective.Positive
            let comparatives = getAs<string []> adjective.Comparatives
            Task(positive, comparatives)

        let task = adjective |> Option.map getTask |> Option.toObj 
        return! Successful.OK task next ctx
    }

let getVerbImperativesTask next (ctx : HttpContext) =
    task {
        let classFromQuery = ctx.GetQueryStringValue "class"
        let classFilter = getFilter "Class" Int classFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilter = getFilter "Pattern" String patternFromQuery

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
        let patternFilter = getFilter "Pattern" String patternFromQuery

        let regularityFromQuery = ctx.GetQueryStringValue "isRegular"
        let regularityFilter = getFilter "IsRegular" Bool regularityFromQuery

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

let webApp = router {
    get "/api/nouns/plurals"      getNounPluralsTask
    get "/api/nouns/accusatives"  getNounAccusativesTask
    get "/api/adjectives/comparatives" getAdjectiveComparativesTask
    get "/api/verbs/imperatives"  getVerbImperativesTask
    get "/api/verbs/participles"  getVerbParticiplesTask
}
