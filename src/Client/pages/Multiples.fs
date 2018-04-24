module Client.Multiples

open Elmish
open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack
open Style
open Fable.PowerPack.Fetch

type Model = {
    Task : string
    Input : string
    Result : string
}

type Msg = 
    | SetInput of string
    | ClickOk
    | FetchedTask of string
    | FetchedAnswer of string
    | FetchError of exn

let getTask() =
    promise {
        let url = "/api/task/"
        return! Fetch.fetchAs<string> url []
    }

let getAnswer() = 
    promise {
        let url = "/api/answer/"
        return! Fetch.fetchAs<string> url []
    }

let loadTaskCmd() =
    Cmd.ofPromise getTask () FetchedTask FetchError

let loadAnswerCmd() =
    Cmd.ofPromise getAnswer () FetchedAnswer FetchError

let init () =
    { Task = ""
      Input = ""
      Result = "" },
      loadTaskCmd()

let update msg model =
    match msg with
    | SetInput input ->
        { model with Input = input }, Cmd.none
    | ClickOk ->
        model, loadAnswerCmd()
    | FetchedTask task ->
        { model with Task = task }, Cmd.none
    | FetchedAnswer answer ->
        let result = if model.Input = answer then "Correct" else "Incorrect"
        { model with Result = result }, Cmd.none
    | FetchError _ ->
        model, Cmd.none

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