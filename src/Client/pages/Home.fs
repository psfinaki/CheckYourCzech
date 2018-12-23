module Home

open Fable.Helpers.React
open Pages

let view () =
    [
        Markup.words 100 "Check Your Czech"
        Markup.words 20 "A service to practice Czech grammar"
        Markup.emptyLines 4

        div [ Styles.rowWide ]
            [
                div [ Styles.thirdParent ]
                    [
                        Markup.wordsCentered "Nouns"
                        Markup.viewLinkCentered Page.Plurals "Make plural"
                        Markup.viewLinkCentered Page.Accusatives "Make accusative"
                    ]
                
                div [ Styles.thirdParent ]
                    [
                        Markup.wordsCentered "Adjectives"
                        Markup.viewLinkCentered Page.Comparatives "Make comparative"
                    ]

                div [ Styles.thirdParent ]
                    [
                        Markup.wordsCentered "Verbs"
                        Markup.viewLinkCentered Page.Imperatives "Make imperative"
                        Markup.viewLinkCentered Page.Participles "Make participle"
                    ]
            ]
    ]
