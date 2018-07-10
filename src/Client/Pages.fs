module Pages

open Elmish.Browser.UrlParser

/// The different pages of the application. If you add a new page, then add an entry here.
[<RequireQualifiedAccess>]
type Page = 
    | Home
    | Plurals
    | Comparatives

let toHash =
    function
    | Page.Home -> "#home"
    | Page.Plurals -> "#plurals"
    | Page.Comparatives -> "#comparatives"

/// The URL is turned into a Result.
let pageParser : Parser<Page -> Page,_> =
    oneOf
        [ map Page.Home (s "home")
          map Page.Plurals (s "plurals")
          map Page.Comparatives (s "comparatives")]

let urlParser location = parseHash pageParser location