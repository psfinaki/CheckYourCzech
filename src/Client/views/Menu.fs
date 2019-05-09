module Menu

open Fable.Helpers.React
open Pages

let view() =
    div [ Styles.center "row" ] 
        [
            Markup.viewLink Page.Home "Home"
            Markup.viewLink Page.NounPlurals "Noun plurals"
            Markup.viewLink Page.NounAccusatives "Noun accusatives"
            Markup.viewLink Page.AdjectiveComparatives "Adjective comparatives"
            Markup.viewLink Page.VerbImperatives "Verb imperatives"
            Markup.viewLink Page.VerbParticiples "Verb participles"
        ]
