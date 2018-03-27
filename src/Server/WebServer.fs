/// Functions for managing the Suave web server.
module ServerCode.WebServer

open Giraffe
open Giraffe.TokenRouter
open RequestErrors

/// Start the web server and connect to database
let webApp root =
    let notfound = NOT_FOUND "Page not found"

    router notfound [
        GET [
            route "/" (htmlFile (System.IO.Path.Combine(root,"index.html")))
        ]
    ]
