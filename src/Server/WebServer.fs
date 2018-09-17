module WebServer

open Giraffe
open Saturn
open Storage

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

        let noun = Storage.getRandom<Noun.Noun> "nouns" filters
        let task = Storage.getAs<string> noun.Singular
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
        let filters = [("Comparatives", IsNot, box [])]
        let adjective = Storage.getRandom<Adjective.Adjective> "adjectives" filters
        let task = Storage.getAs<string> adjective.Positive
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
        let verb = Storage.getRandom<Verb.Verb> "verbs" filters
        let task = Storage.getAs<string> verb.Indicative 
        return! ctx.WriteJsonAsync task
    }

let getImperativesAnswer word : HttpHandler =
    fun _ ctx -> task {
        let filters =  [("Indicative", Is, word)]
        let verb = Storage.getSingle<Verb.Verb> "verbs" filters
        let answer = Storage.getAs<string []> verb.Imperatives
        return! ctx.WriteJsonAsync answer
    }

let webApp = router {
    get  "/api/plurals/task"           getPluralsTask
    getf "/api/plurals/answer/%s"      getPluralsAnswer

    get  "/api/comparatives/task"      getComparativesTask
    getf "/api/comparatives/answer/%s" getComparativesAnswer

    get  "/api/imperatives/task"       getImperativesTask
    getf "/api/imperatives/answer/%s"  getImperativesAnswer
}