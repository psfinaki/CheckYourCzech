module Client.Pages.Home

open Fable.React
open Fable.React.Props
open Fulma

open Client
open Client.AppPages

let linkBlock text colorClass = 
    let firstSymbol: char = Seq.head text
    [
        div [ ClassName (sprintf "link-letter %s" colorClass) ] [str (firstSymbol.ToString())]
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
                            Markup.wordsCentered "home-column-heading" "Nouns"
                            div [ ClassName "home-link-container" ] [
                                Markup.viewLinkCentered "home-column-links link-block" Page.NounDeclension (linkBlock "Declension" "noun-color")
                                Markup.viewLinkCentered "home-column-links link-block" Page.NounPlurals (linkBlock "Plural" "noun-color")
                                Markup.viewLinkCentered "home-column-links link-block" Page.NounAccusatives (linkBlock "Accusative" "noun-color")
                            ]
                        ]
                        Column.column [ ] [
                            Markup.wordsCentered "home-column-heading" "Adjectives"
                            div [ ClassName "home-link-container" ] [
                                Markup.viewLinkCentered "home-column-links link-block" Page.AdjectivePlurals (linkBlock "Plural" "adjective-color")
                                Markup.viewLinkCentered "home-column-links link-block" Page.AdjectiveComparatives (linkBlock "Comparative" "adjective-color")
                            ]
                        ]
                        Column.column [ ] [
                            Markup.wordsCentered "home-column-heading" "Verbs"
                            div [ ClassName "home-link-container" ] [
                                Markup.viewLinkCentered "home-column-links link-block" Page.VerbImperatives (linkBlock "Imperative" "verb-color")
                                Markup.viewLinkCentered "home-column-links link-block" Page.VerbParticiples (linkBlock "Participle" "verb-color")
                                Markup.viewLinkCentered "home-column-links link-block" Page.VerbConjugation (linkBlock "Conjugation" "verb-color")
                            ]
                        ]
                    ]      
            ]
    ]
