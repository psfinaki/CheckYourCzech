module Client.AppPages

open Elmish.UrlParser

[<RequireQualifiedAccess>]
type Page = 
    | Home
    | NounDeclension
    | NounPlurals
    | NounAccusatives
    | AdjectiveDeclension
    | AdjectivePlurals
    | AdjectiveComparatives
    | VerbImperatives
    | VerbParticiples
    | VerbConjugation

let toHash =
    function
    | Page.Home -> "#home"
    | Page.NounDeclension -> "#nouns-declension"
    | Page.NounPlurals -> "#nouns-plurals"
    | Page.NounAccusatives -> "#nouns-accusatives"
    | Page.AdjectiveDeclension -> "#adjectives-declension"
    | Page.AdjectivePlurals -> "#adjectives-plurals"
    | Page.AdjectiveComparatives -> "#adjectives-comparatives"
    | Page.VerbImperatives -> "#verbs-imperatives"
    | Page.VerbParticiples -> "#verbs-participles"
    | Page.VerbConjugation -> "#verbs-conjugation"

let pageParser : Parser<Page -> Page,_> =
    oneOf
        [ map Page.Home (s "home") 
          map Page.NounDeclension (s "nouns-declension")
          map Page.NounPlurals (s "nouns-plurals")
          map Page.NounAccusatives (s "nouns-accusatives")
          map Page.AdjectiveDeclension (s "adjectives-declension")
          map Page.AdjectivePlurals (s "adjectives-plurals")
          map Page.AdjectiveComparatives (s "adjectives-comparatives")
          map Page.VerbImperatives (s "verbs-imperatives")
          map Page.VerbParticiples (s "verbs-participles")
          map Page.VerbConjugation (s "verbs-conjugation") ]
