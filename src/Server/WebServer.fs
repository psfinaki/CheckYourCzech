/// Functions for managing the Suave web server.
module ServerCode.WebServer

open Giraffe
open Giraffe.TokenRouter
open RequestErrors

/// Start the web server and connect to database
let webApp root =
    let notfound = NOT_FOUND "Page not found"

    let getTask () : HttpHandler =
        fun _ ctx -> task { return! ctx.WriteJsonAsync("panda") }

    let getAnswer () : HttpHandler =
        fun _ ctx -> task { return! ctx.WriteJsonAsync("pandy") }

    router notfound [
        GET [
            route "/api/task/" (getTask())
            route "/api/answer/" (getAnswer())
        ]
    ]
