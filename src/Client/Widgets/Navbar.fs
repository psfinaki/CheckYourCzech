module Client.Widgets.Navbar

open Fable.React
open Fable.React.Props
open Fulma
open Fable.FontAwesome

let private navbarView isHome dispatch =
    let backArrow = 
        Icon.icon [ Icon.Size IsSmall ]
            [ i [ ClassName "fas fa-lg fa-arrow-left" ] [ ] ]
    let textLink = 
        Heading.p [ Heading.Is4 ]
            [ str "Check Your Czech" ]
    let contactText = 
        Heading.h5 [ Heading.CustomClass "contact-text" ] 
            [ str "Contact" ]
    let contactIcon = 
        Icon.icon [ Icon.CustomClass "contact-icon"; Icon.Size IsMedium; Icon.IsRight ]
                    [ Fa.i [ Fa.Brand.GithubAlt; Fa.Size Fa.Fa2x ] [] ]
    let contactLink = 
        Navbar.Item.a 
            [ Navbar.Item.Props [ 
                Href "https://github.com/psfinaki/CheckYourCzech"
                Target "_blank" ] ]
            [ contactText; contactIcon ] 

    div [ ClassName "navbar-bg" ]
        [ Navbar.navbar [ ]
                [ Navbar.Brand.div [ ]
                    [ Navbar.Item.a [ Navbar.Item.Props [ Href "#" ] ]
                        [ (if isHome then textLink else backArrow) ] 
                      contactLink
                    ]
                ] 
        ]

let root model dispatch =
    navbarView model dispatch
