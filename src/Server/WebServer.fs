module WebServer

open Giraffe
open Saturn
open Gender

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

let webApp = scope {
    getf "/api/task/%s" getTask
    getf "/api/answer/%s" getAnswer
}