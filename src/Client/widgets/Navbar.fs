module Client.Widgets.Navbar

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma

let private navbarView isHome dispatch =
    let backArrow = 
        Icon.icon [ Icon.Size IsSmall ]
            [ i [ ClassName "fas fa-lg fa-arrow-left" ] [ ] ]
    let textLink = 
        Heading.p [ Heading.Is4 ]
            [ str "Check Your Czech" ]
    div [ ClassName "navbar-bg" ]
        [ Navbar.navbar [ ]
                [ Navbar.Brand.div [ ]
                    [ Navbar.Item.a [ Navbar.Item.Props [ Href "#" ] ]
                        [ (if isHome then textLink else backArrow) ] 
                    ]
                ] 
        ]

let root model dispatch =
    navbarView model dispatch
