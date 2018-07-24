module Home

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Pages

let view () =
    [
        Markup.words 100 "Check Your Czech"
        Markup.words 20 "A service to practice Czech grammar"
        Markup.emptyLines 4

        div [ Styles.rowWide ]
            [
                div [ Styles.halfParent ]
                    [
                        Markup.viewLinkCentered Page.Plurals "Nouns"
                        Markup.wordsCentered "Make plural"
                    ]
                
                div [ Styles.halfParent ]
                    [
                        Markup.viewLinkCentered Page.Comparatives "Adjectives"
                        Markup.wordsCentered "Make comparative"
                    ]
            ]
    ]