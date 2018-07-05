module WebServer

open Giraffe
open Saturn
open Gender

let getTask gender : HttpHandler =
    fun _ ctx -> task { 
        let iterator _ = WordGenerator.getRandomWord()
        let isMatch = Word.isAppropriateNoun (translateTo gender)

        let word = 
            iterator
            |> Seq.initInfinite
            |> Seq.find isMatch

        return! ctx.WriteJsonAsync word
    }

let getAnswer word : HttpHandler =
    fun _ ctx -> task {
        let answer = Noun.getPlural word
        return! ctx.WriteJsonAsync answer 
    }

let webApp = scope {
    getf "/api/task/%s" getTask
    getf "/api/answer/%s" getAnswer
}