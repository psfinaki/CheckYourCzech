module Client.Multiples

open Elmish
open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack
open Style

type Model = {
    Task : string
    Input : string
    Result : string
}

type Msg = 
    | SetInput of string
    | ClickOk
    | FetchedTask of string
    | FetchError of exn

let getTask() =
    promise {
        return "panda"
    }

let loadTaskCmd() =
    Cmd.ofPromise getTask () FetchedTask FetchError

let init () =
    { Task = ""
      Input = ""
      Result = "" },
      loadTaskCmd()

let update msg model =
    match msg with
    | SetInput input ->
        { model with Input = input }
    | ClickOk ->
        let result = if model.Input = "pandy" then "Correct" else "Incorrect"
        { model with Result = result }
    | FetchedTask task ->
        { model with Task = task }
    | FetchError _ ->
        model

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