module Client.Widgets.Navbar.View

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma

open Client
open Client.AppPages
open Types

let private navbarStart dispatch =
    Navbar.Start.div [ ]
        [ Navbar.Item.a [ ]
            [ Markup.viewLink Page.Home "Home" ]
          Navbar.Item.div [ Navbar.Item.HasDropdown
                            Navbar.Item.IsHoverable ]
            [ Navbar.Link.div [ ]
                [ str "Noun" ]
              Navbar.Dropdown.div [ ]
                [ Navbar.Item.a [ ]
                    [ Markup.viewLink Page.NounDeclension "Declension (beta)" ]
                  Navbar.Item.a [ ]
                    [ Markup.viewLink Page.NounPlurals "Plurals" ]
                  Navbar.Item.a [ ]
                    [ Markup.viewLink Page.NounAccusatives "Accusatives" ] 
                ] 
            ] 
          Navbar.Item.div [ Navbar.Item.HasDropdown
                            Navbar.Item.IsHoverable ]
            [ Navbar.Link.div [ ]
                [ str "Adjective" ]
              Navbar.Dropdown.div [ ]
                [ Navbar.Item.a [ ]
                    [ Markup.viewLink Page.AdjectivePlurals "Plurals" ]
                  Navbar.Item.a [ ]
                    [ Markup.viewLink Page.AdjectiveComparatives "Comparatives" ] 
                ] 
            ] 

          Navbar.Item.div [ Navbar.Item.HasDropdown
                            Navbar.Item.IsHoverable ]
            [ Navbar.Link.div [ ]
                [ str "Verb" ]
              Navbar.Dropdown.div [ ]
                [ Navbar.Item.a [ ]
                    [ Markup.viewLink Page.VerbImperatives "Imperatives" ]
                  Navbar.Item.a [ ]
                    [ Markup.viewLink Page.VerbParticiples "Participles" ] 
                  Navbar.Item.a [ ]
                    [ Markup.viewLink Page.VerbConjugation "Conjugation" ]
                ] 
            ] 
        ]

let private navbarView isBurgerOpen dispatch =
    div [ ClassName "navbar-bg" ]
        [ Navbar.navbar [ ]
                [ Navbar.Brand.div [ ]
                    [ Navbar.Item.a [ Navbar.Item.Props [ Href "#" ] ]
                        [ Heading.p [ Heading.Is4 ]
                            [ str "Check Your Czech" ] ] 
                    ]
                ] 
        ]

let root model dispatch =
    navbarView model.isBurgerOpen dispatch
