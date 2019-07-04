module Styles

open Fable.Helpers.React.Props

let middle =
    Style [
        Width "70%"
        Margin "auto"
    ]

let rowWide = 
    Style [ 
        Width 850
    ]
    
let halfParent =
    Style [
        Width "50%"
        Display "inline-block"
        Height "25px"
    ]

let thirdParent =
    Style [
        Width "33%"
        Display "inline-block"
        Height "25px"
        VerticalAlign "top"
    ]

let center direction =
    Style [ 
        Display "flex"
        FlexDirection direction
        AlignItems "center"
        JustifyContent "center"
        Padding "20px 0"
    ]

let greyLabel =
    Style [ 
        Width "32%"
        Height "70px"
        FontSize "25px"
        TextAlign "center"
        BorderRadius "10%"
        LineHeight "2.5"
        VerticalAlign "middle"
        BackgroundColor "LightGray" 
    ]

let whiteLabel =
    Style [ 
        FontSize "20px"
        TextAlign "center"
        Display "block" 
    ]

let input =
    Style [ 
        Width "32%"
        Height "70px"
        FontSize "25px"
        TextAlign "center"
        MarginLeft "2%"
        MarginRight "2%" 
    ] 
    
let button color =
    Style [ 
        Width "49%"
        Height "70px"
        FontSize "25px"
        BorderRadius "33%"
        BackgroundColor color 
    ]

let select =
    Style [ 
        Width "33%"
        Height "50%"
        Margin "auto"
        Padding "0 2%" 
        TextAlign "center"
    ]
