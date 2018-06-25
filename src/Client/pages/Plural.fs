module Plural

open Elmish
open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack
open Style
open Fable.Import.React
open Gender

type Model = {
    Gender : Gender
    Task : string 
    Input : string
    Result : bool option
}

type Msg = 
    | SetGender of Gender
    | SetInput of string
    | SubmitTask
    | UpdateTask
    | FetchedTask of string
    | FetchedAnswer of string[]
    | FetchError of exn

let getTask gender =
    promise {
        let url = "/api/task/" + gender
        return! Fetch.fetchAs<string> url []
    }

let getAnswer task = 
    promise {
        let url = "/api/answer/" + task
        return! Fetch.fetchAs<string[]> url []
    }

let loadTaskCmd gender =
    Cmd.ofPromise getTask (translateFrom gender) FetchedTask FetchError

let loadAnswerCmd task =
    Cmd.ofPromise getAnswer task FetchedAnswer FetchError

let init () =
    { Gender = MasculineAnimate
      Task = ""
      Input = ""
      Result = None },
      loadTaskCmd MasculineAnimate

let update msg model =
    match msg with
    | SetGender gender ->
        { model with Gender = gender }, Cmd.none
    | SetInput input ->
        { model with Input = input }, Cmd.none
    | SubmitTask ->
        model, loadAnswerCmd model.Task
    | UpdateTask ->
        { model with Task = ""; Input = ""; Result = None }, loadTaskCmd model.Gender
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

    let handleKeyDown (event: KeyboardEvent) =
        match event.keyCode with
        | Keyboard.Codes.enter ->
            match event.shiftKey with
            | false -> dispatch SubmitTask
            | true  -> dispatch UpdateTask
        | _ -> 
            ()
    
    [ 
        words 60 "Write plural for the word" 
        br []
        br []
        br []

        div [ Style [ Width 700; Height 70 ] ] [
            div [ Style [ Height "50%" ] ] [
                label [ Style [ FontSize "20px"; TextAlign "center"; Display "block" ] ] [
                    str "Gender"
                ] ]
            div [ Style [ Width "33%"; Height "50%"; Margin "auto"; Padding "0 2%" ] ] [
                select [ OnChange (fun ev -> dispatch (SetGender (translateTo !!ev.target?value))); Style [ BorderRadius "10%"; FontSize "20px" ] ] [
                    option [ Value (translateFrom MasculineAnimate) ] [ str "Masculine Animate" ]
                    option [ Value (translateFrom MasculineInanimate) ] [ str "Masculine Inanimate" ]
                    option [ Value (translateFrom Feminine) ] [ str "Feminine" ]
                    option [ Value (translateFrom Neuter)] [ str "Neuter" ]
                ]
            ]
        ]

        br []
        br []

        div [ Style [ Width 700; Height 70; ] ] [
            label [ Style [ Width "32%"; Height "100%"; FontSize "25px"; TextAlign "center"; BorderRadius "10%"; LineHeight "2.5"; VerticalAlign "middle"; BackgroundColor "LightGray" ] ] [
                str (if model.Task <> "" then model.Task else "-")
            ] 

            input [ 
                Type "text"
                Style [ Width "32%"; Height "100%"; FontSize "25px"; TextAlign "center"; MarginLeft "2%"; MarginRight "2%" ] 
                Value model.Input
                OnChange (fun ev -> dispatch (SetInput !!ev.target?value))
                OnKeyDown handleKeyDown
                AutoFocus true
            ]
            
            label [ Style [ Width "32%"; Height "100%"; FontSize "25px"; TextAlign "center"; BorderRadius "10%"; LineHeight "2.5"; VerticalAlign "middle"; BackgroundColor "LightGray" ] ] [
                resultContent
            ] 
        ]

        br []
        br []

        div [ Style [ Width 700; Height 70 ] ] [
            button [ OnClick (fun _ -> dispatch UpdateTask); Type "button"; Style [ Width "49%"; Height "100%"; FontSize "25px"; BorderRadius "33%"; BackgroundColor "White" ] ] [
                str "Next"
            ]

            div [ Style [ Width "2%"; Display "inline-block" ] ] []

            button [ OnClick (fun _ -> dispatch SubmitTask); Type "button"; Style [ Width "49%"; Height "100%"; FontSize "25px"; BorderRadius "33%"; BackgroundColor "Lime" ] ] [
                str "Check"
            ]
        ]
    ]