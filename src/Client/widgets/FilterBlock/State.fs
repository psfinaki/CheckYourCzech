module Client.Widgets.FilterBlock.State

open Elmish

open Types

let init () =
    { IsHidden = true }

let update msg model =
    match msg with
    | ToggleBlock -> { IsHidden = not model.IsHidden }, Cmd.none
