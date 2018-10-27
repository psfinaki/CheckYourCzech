module WebServer

open Giraffe
open Saturn
open Storage
open FSharp.Control.Tasks.V2

let getPluralsTask : HttpHandler =
    fun _ ctx -> task {
        let pluralsFilter = ("Plurals", IsNot, box [])

        let genderFromQuery = ctx.GetQueryStringValue "gender"
        let genderFilter = 
             match genderFromQuery with
             | Ok gender -> Some ("Gender", Is, box gender)
             | Error _   -> None

        let filters = 
            match genderFilter with
            | Some filter -> [ pluralsFilter; filter ]
            | None        -> [ pluralsFilter ]

        let noun = Storage.tryGetRandom<Noun.Noun> "nouns" filters
        let getSingular (noun : Noun.Noun) = Storage.getAs<string> noun.Singular
        let task = noun |> Option.map getSingular |> Option.toObj
        return! ctx.WriteJsonAsync task
    }

let getPluralsAnswer word : HttpHandler =
    fun _ ctx -> task {
        let filters = [("Singular", Is, word)]
        let noun = Storage.getSingle<Noun.Noun> "nouns" filters
        let answer = Storage.getAs<string []> noun.Plurals
        return! ctx.WriteJsonAsync answer
    }

let getComparativesTask : HttpHandler =
    fun _ ctx -> task { 
        let comparativesFilter = ("Comparatives", IsNot, box [])

        let regularityFromQuery = ctx.GetQueryStringValue "isRegular"
        let regularityFilter = 
             match regularityFromQuery with
             | Ok regularity -> Some ("IsRegular", Bool, box regularity)
             | Error _       -> None

        let filters = 
            match regularityFilter with
            | Some filter -> [ comparativesFilter; filter ]
            | None        -> [ comparativesFilter ]

        let adjective = Storage.tryGetRandom<Adjective.Adjective> "adjectives" filters
        let getPositive (adjective : Adjective.Adjective) = Storage.getAs<string> adjective.Positive
        let task = adjective |> Option.map getPositive |> Option.toObj
        return! ctx.WriteJsonAsync task
    }

let getComparativesAnswer word : HttpHandler =
    fun _ ctx -> task {
        let filters = [("Positive", Is, word)]
        let adjective = Storage.getSingle<Adjective.Adjective> "adjectives" filters
        let answer = Storage.getAs<string []> adjective.Comparatives
        return! ctx.WriteJsonAsync answer
    }

let getImperativesTask : HttpHandler =
    fun _ ctx -> task {
        let filters = [("Imperatives", IsNot, box [])]
        let verb = Storage.tryGetRandom<Verb.Verb> "verbs" filters
        let getImperative (verb : Verb.Verb) = Storage.getAs<string> verb.Indicative
        let task = verb |> Option.map getImperative |> Option.toObj
        return! ctx.WriteJsonAsync task
    }

let getImperativesAnswer word : HttpHandler =
    fun _ ctx -> task {
        let filters =  [("Indicative", Is, word)]
        let verb = Storage.getSingle<Verb.Verb> "verbs" filters
        let answer = Storage.getAs<string []> verb.Imperatives
        return! ctx.WriteJsonAsync answer
    }

let getParticiplesTask : HttpHandler =
    fun _ ctx -> task {
        let filters = [("Participles", IsNot, box [])]
        let verb = Storage.tryGetRandom<Verb.Verb> "verbs" filters
        let getParticiple (verb : Verb.Verb) = Storage.getAs<string> verb.Indicative
        let task = verb |> Option.map getParticiple |> Option.toObj
        return! ctx.WriteJsonAsync task
    }

let getParticiplesAnswer word : HttpHandler =
    fun _ ctx -> task {
        let filters =  [("Indicative", Is, word)]
        let verb = Storage.getSingle<Verb.Verb> "verbs" filters
        let answer = Storage.getAs<string []> verb.Participles
        return! ctx.WriteJsonAsync answer
    }

let webApp = router {
    get  "/api/plurals/task"           getPluralsTask
    getf "/api/plurals/answer/%s"      getPluralsAnswer

    get  "/api/comparatives/task"      getComparativesTask
    getf "/api/comparatives/answer/%s" getComparativesAnswer

    get  "/api/imperatives/task"       getImperativesTask
    getf "/api/imperatives/answer/%s"  getImperativesAnswer

    get  "/api/participles/task"       getParticiplesTask
    getf "/api/participles/answer/%s"  getParticiplesAnswer
}