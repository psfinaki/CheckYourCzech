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
                            div [ ClassName "home-link-container" ] [
                                Markup.viewLinkCentered "home-column-links link-block" Page.NounDeclension (linkBlock "Noun Declension" "noun-color")
                                Markup.viewLinkCentered "home-column-links link-block" Page.AdjectiveDeclension (linkBlock "Adjective Declension" "adjective-color")
                                Markup.viewLinkCentered "home-column-links link-block" Page.AdjectiveComparatives (linkBlock "Adjective Comparative" "adjective-color")
                            ]
                        ]
                        Column.column [ ] [
                            div [ ClassName "home-link-container" ] [
                                Markup.viewLinkCentered "home-column-links link-block" Page.VerbImperatives (linkBlock "Verb Imperative" "verb-color")
                                Markup.viewLinkCentered "home-column-links link-block" Page.VerbParticiples (linkBlock "Verb Participle" "verb-color")
                                Markup.viewLinkCentered "home-column-links link-block" Page.VerbConjugation (linkBlock "Verb Conjugation" "verb-color")
                            ]
                        ]
                    ]      
            ]
    ]
