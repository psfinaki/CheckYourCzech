module Client.Pages

open Elmish.Browser.UrlParser

/// The different pages of the application. If you add a new page, then add an entry here.
[<RequireQualifiedAccess>]
type Page = 
    | Home
    | Plural

let toHash =
    function
    | Page.Home -> "#home"
    | Page.Plural -> "#plural"

/// The URL is turned into a Result.
let pageParser : Parser<Page -> Page,_> =
    oneOf
        [ map Page.Home (s "home")
          map Page.Plural (s "plural")]

let urlParser location = parseHash pageParser location