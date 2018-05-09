/// Functions for managing the Suave web server.
module ServerCode.WebServer

open Giraffe
open Giraffe.TokenRouter
open RequestErrors
open FSharp.Data
open ServerCode

type Wiki = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let webApp root =
    let notfound = NOT_FOUND "Page not found"

    let getTask () : HttpHandler =
        fun _ ctx -> task { 
            let word = WordProvider.getCzechNoun()
            return! ctx.WriteJsonAsync word
        }

    let getAnswer word : HttpHandler =
        fun _ ctx -> task {
            let url = "https://cs.wiktionary.org/wiki/" + word
            let data = Wiki.Load url
            let answer = data.Tables.``Skloňování[editovat]``.Rows.[0].plurál
            return! ctx.WriteJsonAsync answer 
        }

    router notfound [
        GET [
            route "/api/task/" (getTask())
            routef "/api/answer/%s" getAnswer
        ]
    ]
