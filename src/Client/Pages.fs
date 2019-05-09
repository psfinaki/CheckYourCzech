module Pages

open Elmish.Browser.UrlParser

[<RequireQualifiedAccess>]
type Page = 
    | Home
    | NounPlurals
    | NounAccusatives
    | AdjectiveComparatives
    | VerbImperatives
    | VerbParticiples

let toHash =
    function
    | Page.Home -> "#home"
    | Page.NounPlurals -> "#nouns-plurals"
    | Page.NounAccusatives -> "#nouns-accusatives"
    | Page.AdjectiveComparatives -> "#adjectives-comparatives"
    | Page.VerbImperatives -> "#verbs-imperatives"
    | Page.VerbParticiples -> "#verbs-participles"

let pageParser : Parser<Page -> Page,_> =
    oneOf
        [ map Page.Home (s "home") 
          map Page.NounPlurals (s "nouns-plurals")
          map Page.NounAccusatives (s "nouns-accusatives")
          map Page.AdjectiveComparatives (s "adjectives-comparatives")
          map Page.VerbImperatives (s "verbs-imperatives")
          map Page.VerbParticiples (s "verbs-participles") ]
