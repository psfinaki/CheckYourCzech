module Pages

open Elmish.Browser.UrlParser

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
    | Page.Plurals -> "#nouns-plurals"
    | Page.Accusatives -> "#nouns-accusatives"
    | Page.Comparatives -> "#adjectives-comparatives"
    | Page.Imperatives -> "#verbs-imperatives"
    | Page.Participles -> "#verbs-participles"

let pageParser : Parser<Page -> Page,_> =
    oneOf
        [ map Page.Home (s "home") 
          map Page.Plurals (s "nouns-plurals")
          map Page.Accusatives (s "nouns-accusatives")
          map Page.Comparatives (s "adjectives-comparatives")
          map Page.Imperatives (s "verbs-imperatives")
          map Page.Participles (s "verbs-participles") ]
