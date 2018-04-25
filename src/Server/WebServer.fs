/// Functions for managing the Suave web server.
module ServerCode.WebServer

open Giraffe
open Giraffe.TokenRouter
open RequestErrors
open FSharp.Data

type Wiki = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let webApp root =
    let notfound = NOT_FOUND "Page not found"

    let getTask () : HttpHandler =
        fun _ ctx -> task { return! ctx.WriteJsonAsync("panda") }

    let getAnswer () : HttpHandler =
        fun _ ctx -> task {
            let data = Wiki.Load "https://cs.wiktionary.org/wiki/panda"
            let answer = data.Tables.``Skloňování[editovat]``.Rows.[0].plurál
            return! ctx.WriteJsonAsync answer 
        }

    router notfound [
        GET [
            route "/api/task/" (getTask())
            route "/api/answer/" (getAnswer())
        ]
    ]
