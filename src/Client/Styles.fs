module Client.Styles

open Fable.React.Props

let middle =
    Style [
        Width "90%"
        Margin "auto"
    ]

let rowWide = 
    Style [ 
        Width 850
    ]
    
let halfParent =
    Style [
        Width "50%"
        Display DisplayOptions.InlineBlock
    ]

let thirdParent =
    Style [
        Width "33%"
        Display DisplayOptions.InlineBlock
        Height "25px"
        VerticalAlign "top"
    ]

let center direction =
    Style [ 
        Display DisplayOptions.Flex
        FlexDirection direction
        AlignItems AlignItemsOptions.Center
        JustifyContent "center"
    ]

let greyLabel =
    Style [ 
        Width "32%"
        Height "70px"
        FontSize "25px"
        TextAlign TextAlignOptions.Center
        BorderRadius "10%"
        LineHeight "2.5"
        VerticalAlign "middle"
        BackgroundColor "LightGray" 
    ]

let whiteLabel =
    Style [ 
        FontSize "20px"
        TextAlign TextAlignOptions.Center
        Display DisplayOptions.Block 
    ]

let input =
    Style [ 
        Width "32%"
        Height "70px"
        FontSize "25px"
        TextAlign TextAlignOptions.Center
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
        TextAlign TextAlignOptions.Center
    ]
