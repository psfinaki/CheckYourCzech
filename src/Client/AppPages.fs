module Client.AppPages

open Elmish.UrlParser

[<RequireQualifiedAccess>]
type Page = 
    | Home
    | NounDeclension
    | NumeralCardinals
    | AdjectiveDeclension
    | AdjectiveComparatives
    | VerbImperatives
    | VerbParticiples
    | VerbConjugation

let toHash =
    function
    | Page.Home -> "#home"
    | Page.NounDeclension -> "#nouns-declension"
    | Page.NumeralCardinals -> "#numerals-cardinals"
    | Page.AdjectiveDeclension -> "#adjectives-declension"
    | Page.AdjectiveComparatives -> "#adjectives-comparatives"
    | Page.VerbImperatives -> "#verbs-imperatives"
    | Page.VerbParticiples -> "#verbs-participles"
    | Page.VerbConjugation -> "#verbs-conjugation"

let pageParser : Parser<Page -> Page,_> =
    oneOf
        [ map Page.Home (s "home") 
          map Page.NounDeclension (s "nouns-declension")
          map Page.NumeralCardinals (s "numerals-cardinals")
          map Page.AdjectiveDeclension (s "adjectives-declension")
          map Page.AdjectiveComparatives (s "adjectives-comparatives")
          map Page.VerbImperatives (s "verbs-imperatives")
          map Page.VerbParticiples (s "verbs-participles")
          map Page.VerbConjugation (s "verbs-conjugation") ]
