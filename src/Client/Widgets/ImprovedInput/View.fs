module Client.Widgets.ImprovedInput.View

open Browser.Types
open Fable.React.Props
open Fulma

open Types

type Props = {
    InputSize: ISize
    OnKeyDownHandler: KeyboardEvent -> unit
    AutoCapitalize: string
    AutoFocus: bool
    AutoComplete: string
}

let inputView props model dispatch =
    Input.text
        [
            Input.Id model.InputId
            Input.Props [
                OnChange (ChangeInput >> dispatch)
                OnKeyDown props.OnKeyDownHandler
                AutoCapitalize props.AutoCapitalize
                AutoFocus props.AutoFocus
                AutoComplete props.AutoComplete
            ] 
            Input.ValueOrDefault model.Value
            Input.Size props.InputSize
        ]

let root props model dispatch =
    inputView props model dispatch
