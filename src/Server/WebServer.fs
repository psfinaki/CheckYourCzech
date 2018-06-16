module WebServer

open Giraffe
open Giraffe.TokenRouter
open RequestErrors
open Gender

let webApp root =
    let notfound = NOT_FOUND "Page not found"

    let getTask gender : HttpHandler =
        fun _ ctx -> task { 
            let word = WordProvider.getNoun (translateTo gender)
            return! ctx.WriteJsonAsync word
        }

    let getAnswer word : HttpHandler =
        fun _ ctx -> task {
            let answer = AnswerProvider.getPlural word
            return! ctx.WriteJsonAsync answer 
        }

    router notfound [
        GET [
            routef "/api/task/%s" getTask
            routef "/api/answer/%s" getAnswer
        ]
    ]
