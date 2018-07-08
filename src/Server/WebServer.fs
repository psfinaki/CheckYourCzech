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

let getTask gender : HttpHandler =
    fun _ ctx -> task { 
        let isAppropriateNoun gender word = 
            Word.isNoun word
            && Noun.hasGender gender word
            && Noun.hasPlural word

        let matchRule = isAppropriateNoun (translateTo gender)
        let word = getWord ctx matchRule
        return! ctx.WriteJsonAsync word
    }

let getAnswer word : HttpHandler =
    fun _ ctx -> task {
        let answer = Noun.getPlural word
        return! ctx.WriteJsonAsync answer 
    }

let getTaskComparative() : HttpHandler =
    fun _ ctx -> task { 
        let matchRule = Word.isAdjective
        let word = getWord ctx matchRule
        return! ctx.WriteJsonAsync word
    }

let getAnswerComparative word : HttpHandler =
    fun _ ctx -> task {
        let answer = Adjective.getComparative word
        return! ctx.WriteJsonAsync answer 
    }

let webApp = scope {
    getf "/api/task/%s" getTask
    getf "/api/answer/%s" getAnswer

    get "/api/task-comparative/" (getTaskComparative())
    getf "/api/answer-comparative/%s" getAnswerComparative
}