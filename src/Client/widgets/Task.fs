module Task

open Elmish
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack
open Fable.Import.React
open Fable.Core.JsInterop
open Fulma
open Markup

type Task = { 
    Word : string 
    Answers : string[]
}

type Model = {
    TaskName: string
    Word : string option
    Answers : string[] option
    Input : string
    Result : bool option
    AnswerShown: bool
}

type Msg = 
    | SetInput of string
    | SubmitAnswer
    | UpdateTask
    | ShowAnswer
    | FetchedTask of Task option
    | FetchError of exn

let log taskName task answer result = 
    let message = sprintf "Task name:%s; Task:%s; Answer:%s; Result:%b" taskName task answer result
    Logger.log message

let loadTaskCmd getTask =
    Cmd.ofPromise getTask [] FetchedTask FetchError

let init taskName getTask =
    { TaskName = taskName
      Word = None
      Answers = None
      Input = ""
      Result = None
      AnswerShown = false },
      loadTaskCmd getTask

let update msg model getTask =
    match msg with
    | SetInput input ->
        { model with Input = input }, Cmd.none
    | SubmitAnswer -> 
        let result = model.Answers |> Option.map (Array.contains model.Input)
   
        if model.Word.IsSome && result.IsSome
        then log model.TaskName model.Word.Value model.Input result.Value
        
        { model with Result = result }, Cmd.none
    | UpdateTask ->
        { model with Word = None; Input = ""; Result = None }, loadTaskCmd getTask
    | ShowAnswer ->
        let answer = model.Answers
                    |> Option.map Array.tryHead
                    |> Option.flatten
                    |> Option.defaultValue ""
        { model with Input = answer; Result = Some false; AnswerShown = true}, Cmd.none
    | FetchedTask task -> 
        { model with 
            Word = task |> Option.map (fun t -> t.Word)
            Answers = task |> Option.map (fun t -> t.Answers)
            AnswerShown = false
        }, Cmd.none
    | FetchError _ ->
        model, Cmd.none

let view model dispatch =
    let result = 
        match model.Result with 
        | Some result -> 
            let imageSource = if result then "images/correct.png" else "images/incorrect.png"
            let altText = if result then "Correct" else "Incorrect"
            icon imageSource 25 altText
        | None ->
            str ""

    let task = 
        match model.Word with
        | Some t -> 
            str t
        | None ->
            let imageSource = "images/loading.gif"
            let altText = "Loading..."
            icon imageSource 25 altText

    let handleChangeAnswer (event: FormEvent) =
        dispatch (SetInput !!event.target?value)
           
    let handleKeyDown (event: KeyboardEvent) =
        match event.keyCode with
        | Keyboard.Codes.enter when model.Word.IsSome ->
            match event.shiftKey with
            | false -> dispatch SubmitAnswer
            | true  -> dispatch UpdateTask
        | Keyboard.Codes.ctrl ->
            match event.shiftKey with
            | false -> ()
            | true  -> dispatch ShowAnswer
        | _ -> 
            ()

    let handleShowAnswerClick _ = dispatch ShowAnswer
    let handleUpdateClick _ = dispatch UpdateTask
    let handleCheckClick _ = dispatch SubmitAnswer
    
    let nextButtonDisabled = not model.Word.IsSome
    let checkButtonDisabled = model.Word.IsNone || model.AnswerShown

    div []
        [
            div [] 
                [
                    label Styles.greyLabel task
                    input Styles.input model.Input handleChangeAnswer handleKeyDown
                    label Styles.greyLabel result
                ]


            div []
                [
                    Button.button [
                        Button.Modifiers [ Modifier.IsPulledRight ]
                        Button.Size IsSmall
                        Button.Color IsPrimary
                        Button.IsOutlined
                        Button.Disabled true
                        ]  
                        [ Icon.icon [ Icon.Size IsSmall ]
                            [ i [ ClassName "fas fa-home" ] [ ] ]
                          str "Show answer (⇧ + Ctrl)" 
                        ]
                ]

            Columns.columns [ Columns.IsGap (Screen.All, Columns.Is2) ]
                [
                    Column.column [ ]
                        [ Button.button [
                            Button.Props [ OnClick handleUpdateClick ]
                            Button.Size IsMedium
                            Button.Disabled nextButtonDisabled
                            Button.IsFullWidth
                            Button.CustomClass "task-button"
                            ]  
                            [ str "Next (⇧ + ⏎)" ] 
                        ]
                    
                    Column.column [ ]
                        [ Button.button [
                            Button.Props [ OnClick handleCheckClick ]
                            Button.Size IsMedium
                            Button.Color IsSuccess
                            Button.Disabled checkButtonDisabled
                            Button.IsFullWidth
                            Button.CustomClass "task-button"
                            ]  
                            [ str "Check (⏎)" ] 
                        ]
                    
                ]
        ]
