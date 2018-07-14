module WebServer

open Giraffe
open Saturn
open Gender
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Http

let getWord (ctx: HttpContext) matchRule =
    let logger = ctx.GetLogger()

    let iterator _ = 
        let word = WordGenerator.getRandomWord()
        logger.Log(LogLevel.Information, word)
        word

    iterator
    |> Seq.initInfinite
    |> Seq.find matchRule

let getPluralsTask() : HttpHandler =
    fun _ ctx -> task { 
        let genderFromQuery = ctx.GetQueryStringValue "gender"
        let gender =
            match genderFromQuery with
            | Ok g -> g 
            | Error _ -> failwith "Currently gender must not be unset"

        let isAppropriateNoun gender word = 
            Word.isNoun word
            && Noun.hasGender gender word
            && Noun.hasPlural word

        let matchRule = isAppropriateNoun (Gender.FromString gender)
        let word = getWord ctx matchRule
        return! ctx.WriteJsonAsync word
    }

let getPluralsAnswer word : HttpHandler =
    fun _ ctx -> task {
        let answer = Noun.getPlural word
        return! ctx.WriteJsonAsync answer 
    }

let getComparativesTask() : HttpHandler =
    fun _ ctx -> task { 
        let matchRule = Word.isAdjective
        let word = getWord ctx matchRule
        return! ctx.WriteJsonAsync word
    }

let getComparativesAnswer word : HttpHandler =
    fun _ ctx -> task {
        let answer = Adjective.getComparative word
        return! ctx.WriteJsonAsync answer 
    }

let webApp = scope {
    get "/api/plurals/task"           (getPluralsTask())
    getf "/api/plurals/answer/%s"     getPluralsAnswer

    get  "/api/comparatives/task/"     (getComparativesTask())
    getf "/api/comparatives/answer/%s" getComparativesAnswer
}