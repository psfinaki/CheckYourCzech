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
    Result : bool option
}

type Msg = 
    | SetInput of string
    | SubmitTask
    | UpdateTask
    | FetchedTask of string
    | FetchedAnswer of string[]
    | FetchError of exn

let getTask() =
    promise {
        let url = "/api/task/"
        return! Fetch.fetchAs<string> url []
    }

let getAnswer task = 
    promise {
        let url = "/api/answer/" + task
        return! Fetch.fetchAs<string[]> url []
    }

let loadTaskCmd() =
    Cmd.ofPromise getTask () FetchedTask FetchError

let loadAnswerCmd task =
    Cmd.ofPromise getAnswer task FetchedAnswer FetchError

let init () =
    { Task = ""
      Input = ""
      Result = None },
      loadTaskCmd()

let update msg model =
    match msg with
    | SetInput input ->
        { model with Input = input }, Cmd.none
    | SubmitTask ->
        model, loadAnswerCmd model.Task
    | UpdateTask ->
        { model with Task = ""; Input = ""; Result = None }, loadTaskCmd()
    | FetchedTask task ->
        { model with Task = task }, Cmd.none
    | FetchedAnswer answer ->
        let result = answer |> Array.contains model.Input
        { model with Result = Some result }, Cmd.none
    | FetchError _ ->
        model, Cmd.none

let view model dispatch =
    let resultContent = 
        match model.Result with 
        | Some result -> 
            let imageSource = if result then "/images/correct.png" else "/images/incorrect.png"
            let altText = if result then "Correct" else "Incorrect"
            img [ 
                HTMLAttr.Src imageSource
                HTMLAttr.Width 25
                HTMLAttr.Height 25 
                HTMLAttr.Alt altText
            ]
        | None ->
            str "-"
    
    [ 
        words 60 "Write multiple for the word" 
        br []
        br []

        div [] [
            label [ Style [ Width 100; TextAlign "right"; BackgroundColor "grey" ] ] [
                str (if model.Task <> "" then model.Task else "-")
            ] 

            input [ 
                Type "text"
                Style [ Width 100; TextAlign "center"; Margin 5 ] 
                Value model.Input
                OnChange (fun ev -> dispatch (SetInput !!ev.target?value))
                onEnter SubmitTask dispatch
                AutoFocus true
            ]
            
            label [ Style [ Width 100; TextAlign "left"; BackgroundColor "grey" ] ] [
                resultContent
            ] 
        ]

        br []

        div [] [
            button [ OnClick (fun _ -> dispatch UpdateTask); Type "button"; Style [ Width 150 ] ] [
                str "Next"
            ]

            div [ Style [ Width 10; Display "inline-block" ] ] []

            button [ OnClick (fun _ -> dispatch SubmitTask); Type "button"; Style [ Width 150 ] ] [
                str "Check"
            ]
        ]
    ]