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

let getAzureFilter columnName queryCondition = function
    | Ok parameterValue -> Some (columnName, queryCondition, box parameterValue)
    | Error _ -> None

let getPostFilter filterCondition = function
    | Ok parameterValue -> Some (filterCondition parameterValue)
    | Error _ -> None

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

let getAdjectivePluralsTask next (ctx : HttpContext) =
    task { 
        let adjective = tryGetRandom<AdjectivePlural.AdjectivePlural> "adjectiveplurals" []
    
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

let webApp = router {
    get "/api/nouns/plurals" getNounPluralsTask
    get "/api/nouns/accusatives" getNounAccusativesTask
    get "/api/adjectives/plurals" getAdjectivePluralsTask
    get "/api/adjectives/comparatives" getAdjectiveComparativesTask
    get "/api/verbs/imperatives" getVerbImperativesTask
    get "/api/verbs/participles" getVerbParticiplesTask
}
