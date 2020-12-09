module Client.Pages.Home

open Fable.React
open Fable.React.Props
open Fulma

open Client
open Client.AppPages

let linkBlock text colorClass firstSymbol =
    [
        div [ ClassName $"link-letter {colorClass}" ] [str $"{firstSymbol}"]
        div [ ClassName colorClass] [str text]
    ]

let view () =
    [
        Markup.words "home-heading is-hidden-mobile" "Check Your Czech 🇨🇿"
        Markup.words "home-subheading" "A service to practice Czech grammar"

        div [ ClassName "home-columns" ]
            [
                Columns.columns [ Columns.IsGap (Screen.All, Columns.Is3) ]
                    [
                        Column.column [ ] [
                            div [ ClassName "home-link-container" ] [
                                Markup.viewLinkCentered "home-column-links link-block" Page.NounDeclension (linkBlock "Noun Declension" "noun-color" "N")
                                Markup.viewLinkCentered "home-column-links link-block" Page.AdjectiveComparatives (linkBlock "Adjective Comparative" "adjective-color" "A")
                                Markup.viewLinkCentered "home-column-links link-block" Page.AdjectiveDeclension (linkBlock "Adjective Declension" "adjective-color" "A")
                            ]
                        ]
                        Column.column [ ] [
                            div [ ClassName "home-link-container" ] [
                                Markup.viewLinkCentered "home-column-links link-block" Page.VerbConjugation (linkBlock "Verb Conjugation" "verb-color" "V")
                                Markup.viewLinkCentered "home-column-links link-block" Page.VerbImperatives (linkBlock "Verb Imperative" "verb-color" "V")
                                Markup.viewLinkCentered "home-column-links link-block" Page.VerbParticiples (linkBlock "Verb Participle" "verb-color" "V")
                            ]
                        ]
                        Column.column [ ] [
                            div [ ClassName "home-link-container" ] [
                                Markup.viewLinkCentered "home-column-links link-block" Page.NumeralCardinals (linkBlock "Numerals Cardinals" "numeral-color" "123")
                            ]
                        ]
                    ]      
            ]
    ]
