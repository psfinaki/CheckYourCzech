module Markup

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Core.JsInterop
open Fable.Import
open Elmish.Browser.Navigation

let goToUrl (e: React.MouseEvent) =
    e.preventDefault()
    let href = !!e.target?href
    Navigation.newUrl href |> List.map (fun f -> f ignore) |> ignore

let viewLink page description =
    a [ 
        Style [ Padding "0 20px" ]
        Href (Pages.toHash page)
        OnClick goToUrl 
    ] [ 
        str description
    ]

let words size message =
    span [ Style [ FontSize (size |> sprintf "%dpx") ] ] [ str message ]

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
        
let button style handler text =
    button [ 
        OnClick handler
        Type "button"
        style
    ] [
        str text
    ]

let space() =
    div [ Style [ Width "2%"; Display "inline-block" ] ] []

let label style content =
    label [ style ] [ content ]

let input style value changeHandler keyDownHandler = 
    input [ 
        Type "text"
        style
        Value value
        OnChange changeHandler
        OnKeyDown keyDownHandler
        AutoFocus true
    ]

let option value text =
    option [ Value value] [ str text ]

let select changeHandler choices =
    select [ OnChange changeHandler; Style [ BorderRadius "10%"; FontSize "20px" ] ] choices