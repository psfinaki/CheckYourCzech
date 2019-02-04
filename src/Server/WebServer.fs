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

let getPluralsTask next (ctx: HttpContext) =
    task {
        let singularsFilter = ("Singulars", IsNot, box [])
        let pluralsFilter = ("Plurals", IsNot, box [])

        let genderFromQuery = ctx.GetQueryStringValue "gender"
        let genderFilter = 
             match genderFromQuery with
             | Ok gender -> Some ("Gender", Is, box gender)
             | Error _   -> None

        let filters = 
            match genderFilter with
            | Some filter -> [ singularsFilter; pluralsFilter; filter ]
            | None        -> [ singularsFilter; pluralsFilter ]

        let noun = tryGetRandom<Noun.Noun> "nouns" filters
        let getTask (noun: Noun.Noun) = 
            let singular = noun.Singulars |> getAs<string[]> |> Seq.random
            let plurals = getAs<string []> noun.Plurals
            Task(singular, plurals)
        
        let task = noun |> Option.map getTask |> Option.toObj
        return! Successful.OK task next ctx
    }

let getAccusativesTask next (ctx : HttpContext) =
    task {
        let singularsFilter = ("Singulars", IsNot, box [])
        let accusativesFilter = ("Accusatives", IsNot, box [])

        let genderFromQuery = ctx.GetQueryStringValue "gender"
        let genderFilter = 
             match genderFromQuery with
             | Ok gender -> Some ("Gender", Is, box gender)
             | Error _   -> None

        let filters = 
            match genderFilter with
            | Some filter -> [ singularsFilter; accusativesFilter; filter ]
            | None        -> [ singularsFilter; accusativesFilter ]

        let noun = tryGetRandom<Noun.Noun> "nouns" filters
        let getTask (noun: Noun.Noun) = 
            let singular = noun.Singulars |> getAs<string[]> |> Seq.random
            let accusatives = getAs<string []> noun.Accusatives
            Task(singular, accusatives)

        let task = noun |> Option.map getTask |> Option.toObj
        return! Successful.OK task next ctx
    }

let getComparativesTask next (ctx : HttpContext) =
    task { 
        let regularityFromQuery = ctx.GetQueryStringValue "isRegular"
        let filters = 
             match regularityFromQuery with
             | Ok regularity -> [ ("IsRegular", Bool, box regularity) ]
             | Error _       -> []

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
        let filters = 
             match classFromQuery with
             | Ok ``class``  -> [ ("Class", Int, box ``class``) ]
             | Error _       -> []
            
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
        let filters = 
             match regularityFromQuery with
             | Ok regularity -> [ ("IsRegular", Bool, box regularity) ]
             | Error _       -> []

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
