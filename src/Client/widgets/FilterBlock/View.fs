module Client.Widgets.FilterBlock.View

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.FontAwesome
open Fulma

open Types

let private filterBlockView isHidden dispatch children =
    div [ ClassName ("filter-block " + if isHidden then "children-hidden" else "") ]
        [ 
          div [ ClassName "filter-block-header"; OnClick (fun _ -> dispatch ToggleBlock)] [
            Fa.i [ 
                   Fa.Solid.AngleDown
                   Fa.Size Fa.FaSmall] 
                   [ ]
            span [ClassName "filter-block-text" ] [str "filters"]
          ]
          div [ClassName "filter-block-children"] children
                 
        ] 

let root model  =
    filterBlockView model.IsHidden
