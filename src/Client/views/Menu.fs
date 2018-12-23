module Menu

open Fable.Helpers.React
open Pages

let view () =
    div [ Styles.center "row" ] 
        [
            yield Markup.viewLink Page.Home "Home"
            yield Markup.viewLink Page.Plurals "Plurals"
            yield Markup.viewLink Page.Accusatives "Accusatives"
            yield Markup.viewLink Page.Comparatives "Comparatives"
            yield Markup.viewLink Page.Imperatives "Imperatives"
            yield Markup.viewLink Page.Participles "Participles"
        ]
