module Home

open Fable.Helpers.React
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma
open Pages

let view () =
    [
        Markup.words "home-heading is-hidden-mobile" "Check Your Czech 🇨🇿"
        Markup.words "home-subheading" "A service to practice Czech grammar"

        div [ ClassName "home-columns" ]
            [
                Columns.columns [ Columns.IsGap (Screen.All, Columns.Is3) ]
                    [
                        Column.column [ ] [
                            Markup.wordsCentered "home-column-heading" "Nouns"
                            Markup.viewLinkCentered "home-column-links" Page.NounPlurals "Make plural"
                            Markup.viewLinkCentered "home-column-links" Page.NounAccusatives "Make accusative"
                        ]
                        Column.column [ ] [
                            Markup.wordsCentered "home-column-heading" "Adjectives"
                            Markup.viewLinkCentered "home-column-links" Page.AdjectivePlurals "Make plural"
                            Markup.viewLinkCentered "home-column-links" Page.AdjectiveComparatives "Make comparative"
                        ]
                        Column.column [ ] [
                            Markup.wordsCentered "home-column-heading" "Verbs"
                            Markup.viewLinkCentered "home-column-links" Page.VerbImperatives "Make imperative"
                            Markup.viewLinkCentered "home-column-links" Page.VerbParticiples "Make participle"
                        ]
                    ]      
            ]
    ]
