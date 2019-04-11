module Menu

open Fable.Helpers.React
open Pages

let view() =
    div [ Styles.center "row" ] 
        [
            Markup.viewLink Page.Home "Home"
            Markup.viewLink Page.Plurals "Plurals"
            Markup.viewLink Page.Accusatives "Accusatives"
            Markup.viewLink Page.Comparatives "Comparatives"
            Markup.viewLink Page.Imperatives "Imperatives"
            Markup.viewLink Page.Participles "Participles"
        ]
