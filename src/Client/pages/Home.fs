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
                        Markup.viewLinkCentered Page.NounPlurals "Make plural"
                        Markup.viewLinkCentered Page.NounAccusatives "Make accusative"
                    ]
                
                div [ Styles.thirdParent ]
                    [
                        Markup.wordsCentered "Adjectives"
                        Markup.viewLinkCentered Page.AdjectivePlurals "Make plural"
                        Markup.viewLinkCentered Page.AdjectiveComparatives "Make comparative"
                    ]

                div [ Styles.thirdParent ]
                    [
                        Markup.wordsCentered "Verbs"
                        Markup.viewLinkCentered Page.VerbImperatives "Make imperative"
                        Markup.viewLinkCentered Page.VerbParticiples "Make participle"
                    ]
            ]
    ]
