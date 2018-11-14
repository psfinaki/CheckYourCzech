module Pages

open Elmish.Browser.UrlParser

/// The different pages of the application. If you add a new page, then add an entry here.
[<RequireQualifiedAccess>]
type Page = 
    | Home
    | Plurals
    | Accusatives
    | Comparatives
    | Imperatives
    | Participles

let toHash =
    function
    | Page.Home -> "#home"
    | Page.Plurals -> "#plurals"
    | Page.Accusatives -> "#accusatives"
    | Page.Comparatives -> "#comparatives"
    | Page.Imperatives -> "#imperatives"
    | Page.Participles -> "#participles"

/// The URL is turned into a Result.
let pageParser : Parser<Page -> Page,_> =
    oneOf
        [ map Page.Home (s "home")
          map Page.Plurals (s "plurals")
          map Page.Accusatives (s "accusatives")
          map Page.Comparatives (s "comparatives")
          map Page.Imperatives (s "imperatives")
          map Page.Participles (s "participles")]

let urlParser location = parseHash pageParser location