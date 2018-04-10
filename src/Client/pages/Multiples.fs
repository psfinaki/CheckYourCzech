module Client.Multiples

open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Style

type Model = {
    Task : string
    Input : string
    Result : string
}

type Msg = 
    | SetInput of string
    | ClickOk

let init () =
    { Task = "panda"
      Input = ""
      Result = "" }

let update msg model =
    match msg with
    | SetInput input ->
        { model with Input = input }
    | ClickOk ->
        let result = if model.Input = "pandy" then "Correct" else "Incorrect"
        { model with Result = result }

let view model dispatch =
    [ 
      words 60 "Write multiple for the word" 
      form [] [
        div [ClassName ("form-group")] [
            label [] [
                str model.Task
            ]

            input [ 
                HTMLAttr.Type "text"
                DefaultValue model.Input
                OnChange (fun ev -> dispatch (SetInput !!ev.target?value))
            ]

            button [ OnClick (fun _ -> dispatch ClickOk) ] [
                str "OK"
            ]

            label [] [
                str model.Result
            ]

            button [] [
                str "Repeat"
            ]
        ]
      ]
    ]