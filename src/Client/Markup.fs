module Client.Markup

open System
open Browser.Dom
open Browser.Types
open Fable.React
open Fable.React.Props
open Fable.Core.JsInterop
open Elmish.Navigation
open Fulma

open Client.AppPages

type Clickability =
    | Clickable
    | Unclickable

let goToUrl (e: MouseEvent) =
    e.preventDefault()
    let href = !!e.currentTarget?href
    Navigation.newUrl href |> List.map (fun f -> f ignore) |> ignore

let viewLinkCentered className page children =
    a [ 
        ClassName className
        Href (toHash page)
        OnClick goToUrl 
    ] children

let viewLink page description =
    a [ 
        Style [ Padding "0 20px" ]
        Href (toHash page)
        OnClick goToUrl 
    ] [ 
        str description
    ]

let words className message =
    span [ ClassName className ] [ str message ]

let wordsCentered className message =
    div [ ClassName className ] [ str message ] 

let emptyLines count =
    br []
    |> List.replicate count
    |> span []

let icon source length text =
    img [ 
        Src source
        HTMLAttr.Width length
        HTMLAttr.Height length
        Alt text
    ]

// inspiration: https://www.w3schools.com/howto/howto_js_toggle_hide_show.asp
let toggleLink text content =
    let id = $"{Guid.NewGuid()}"

    let toggle _ = 
        let spoiler = document.getElementById id
        let display = if spoiler?style?display = "none" then "block" else "none"
        spoiler?style?display <- display

    div [] 
        [
            // inspiration: https://stackoverflow.com/a/1368286/3232646
            button [
                OnClick toggle
                Style [
                    FontSize "25px"
                    Display DisplayOptions.Block
                    Margin "0 auto"
                    Background "none"
                    Border "none"
                    Padding 0
                    Color "#069"
                    TextDecoration "Underline"
                    Cursor "pointer"
                ]
            ] [
                str text
            ]

            div [ Id id; Style [ Display DisplayOptions.None ] ]
                [
                    pre [] [ str content ]
                ]
        ]

let button size color onclick text otherOptions =
        let options = otherOptions @ 
                        [
                            Button.Props [ OnClick onclick ]
                            Button.Size size
                            Button.Color color
                        ]
        Button.button options [ str text ]


let space() =
    div [ Style [ Width "2%"; Display DisplayOptions.InlineBlock ] ] []

let input style value changeHandler keyDownHandler = 
    input [ 
        Type "text"
        style
        Value value
        OnChange changeHandler
        OnKeyDown keyDownHandler
        AutoFocus true
    ]

let simpleOption text =
    option [ Value text ] [ str text ]

let option value text =
    option [ Value value] [ str text ]

let select selectedValue changeHandler choices =
    select [ Value selectedValue; OnChange changeHandler; ] choices
