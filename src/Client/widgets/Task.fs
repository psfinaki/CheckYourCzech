﻿module Task

open Elmish
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack
open Fable.Import.React
open Fable.Core.JsInterop
open Fable.FontAwesome
open Fable.FontAwesome.Free
open Fulma
open Markup
open Utils

[<Literal>] 
let DefaultWord = ""
let DefaultAnswers = [||]

type Task = { 
    Word : string 
    Answers : string[]
}

type InputState = 
    | Unknown
    | Wrong
    | Right
    | Shown

type Input = string
type TaskWithInput = Task * Input * InputState

type State = 
    | Fetching
    | TaskProvided of Task
    | InputProvided of TaskWithInput

type Model = {
    TaskName: string
    State: State
}

type Msg = 
    | SetAnswer of string
    | ShowAnswer
    | CheckAnswer
    | NextQuestion
    | FetchedTask of Task option
    | FetchError of exn

type LeftButtonState = Show | Next

let log taskName task answer result = 
    let message = sprintf "Task name:%s; Task:%s; Answer:%s; Result:%b" taskName task answer result
    Logger.log message

let loadTaskCmd getTask =
    Cmd.ofPromise getTask [] FetchedTask FetchError

let getStateTask s = 
    match s with 
    | Fetching -> { Word = DefaultWord; Answers = DefaultAnswers}
    | TaskProvided t -> t
    | InputProvided (t, _, _) -> t

let getTaskIcon c = 
    match c with 
    | "task-input-correct"   -> Fa.Solid.CheckCircle
    | "task-input-incorrect" -> Fa.Solid.TimesCircle
    | "task-input-none"      -> Fa.Solid.QuestionCircle
    | _                      -> invalidArg "c" "Only certain type of icons could be mapped"

let checkAnswer model = 
    match model.State with 
        | Fetching -> model
        | TaskProvided _ -> model
        | InputProvided (t, inp, inpState) -> 
            let checkAnswer' () = 
                let result = t.Answers |> Array.contains inp
                log model.TaskName t.Word inp result
                let nextState = if result then Right else Wrong
                { model with State = InputProvided (t, inp, nextState) }

            match inpState with
            | Shown -> model
            | _ -> checkAnswer' ()

let init taskName getTask =
    { TaskName = taskName
      State = Fetching },
      loadTaskCmd getTask

let update msg model getTask =
    match msg with
    | FetchedTask task -> 
        { model with 
            State = TaskProvided { 
                        Word = task |> Option.map (fun t -> t.Word) |> Option.defaultValue DefaultWord; 
                        Answers = task |> Option.map (fun t -> t.Answers) |> Option.defaultValue DefaultAnswers
                    }
        }, Cmd.none
    | FetchError _ ->
        model, Cmd.none
    | SetAnswer input ->
        { model with State = InputProvided (getStateTask model.State, input, Unknown) }, Cmd.none
    | ShowAnswer ->
        let task = getStateTask model.State
        let answer = task.Answers |> Array.tryHead |> Option.defaultValue DefaultWord
        { model with State = InputProvided (task, answer, Shown) }, Cmd.none
    | CheckAnswer -> 
        checkAnswer model, Cmd.none
    | NextQuestion ->
        { model with State = Fetching }, loadTaskCmd getTask

let view model dispatch =
    let inputClass =
        match model.State with
        | InputProvided (_, _, inpState) ->
            match inpState with
            | Right -> "task-input-correct"
            | Wrong -> "task-input-incorrect"
            | _ -> "task-input-none"
        | _ -> "task-input-none"
    let inputIcon = getTaskIcon inputClass

    let word = 
        match model.State with
        | Fetching -> 
            let imageSource = "images/loading.gif"
            let altText = "Loading..."
            icon imageSource 25 altText
        | t -> getStateTask t |>  (fun t -> t.Word) |> str
    
    let leftButtonState = 
        match model.State with
        | InputProvided (_, _, inpState) when inpState = Shown || inpState = Right ->
            Next
        | _ ->
            Show

    let handleChangeAnswer (event: FormEvent) =
        dispatch (SetAnswer !!event.target?value)
           
    let handleKeyDown (event: KeyboardEvent) =
        match model.State with
        | Fetching -> ()
        | _ ->
            match event.keyCode with
            | Keyboard.Codes.enter ->
                match event.shiftKey with
                | false -> dispatch CheckAnswer
                | true  -> 
                    match leftButtonState with
                    | Next -> dispatch NextQuestion
                    | Show -> dispatch ShowAnswer
            | _ -> 
                ()

    let handleShowAnswerClick _ = dispatch ShowAnswer
    let handleUpdateClick _ = dispatch NextQuestion
    let handleCheckClick _ = dispatch CheckAnswer

    let nextButtonDisabled = 
        match model.State with
        | Fetching -> true
        | _ -> false

    let checkButtonDisabled = 
        match model.State with
        | Fetching -> true
        | InputProvided (_, _, inpState) when inpState = Shown -> true
        | _ -> false

    let inputText = 
        match model.State with
        | InputProvided (_, i, _) -> i
        | _ -> ""

    let leftButton = 
        match leftButtonState with
        | Next ->
            button IsMedium NoColor handleUpdateClick "Next (⇧ + ⏎)"   
                [
                    Button.Disabled nextButtonDisabled
                    Button.CustomClass "task-button"
                ]
        | Show ->
            button IsMedium NoColor handleShowAnswerClick "Show (⇧ + ⏎)"   
                [
                    Button.CustomClass "task-button"
                ]

    div []
        [
            Columns.columns [ Columns.IsGap (Screen.All, Columns.Is3) ]
                [
                    Column.column [ ] [Tag.tag [ Tag.Color IsLight ; Tag.CustomClass "task-label" ] [word] ]
                    Column.column [ ] [
                                        div [ClassName ("control has-icons-right " + inputClass)]
                                            [
                                                Input.text
                                                    [
                                                        Input.Props [OnChange handleChangeAnswer; OnKeyDown handleKeyDown; AutoCapitalize "none"] 
                                                        Input.Value inputText
                                                        Input.Size Size.IsLarge
                                                    ]
                                                Icon.icon [ Icon.Size IsSmall; Icon.IsRight ]
                                                    [ Fa.i [ inputIcon ] [] ]
                                            ]                     
                                      ]
                ]

            div [ ClassName "task-buttons-container" ]
                [
                    leftButton
                    button IsMedium IsSuccess handleCheckClick "Check (⏎)"   
                        [
                            Button.Disabled checkButtonDisabled
                            Button.CustomClass "task-button"
                        ]
                    
                ]
        ]
