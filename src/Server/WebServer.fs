/// Functions for managing the Suave web server.
module ServerCode.WebServer

open Giraffe
open Giraffe.TokenRouter
open RequestErrors
open ServerCode

let webApp root =
    let notfound = NOT_FOUND "Page not found"

    let getTask () : HttpHandler =
        fun _ ctx -> task { 
            let word = WordProvider.getNoun()
            return! ctx.WriteJsonAsync word
        }

    let getAnswer word : HttpHandler =
        fun _ ctx -> task {
            let answer = AnswerProvider.getMultiple word
            return! ctx.WriteJsonAsync answer 
        }

    router notfound [
        GET [
            route "/api/task/" (getTask())
            routef "/api/answer/%s" getAnswer
        ]
    ]
