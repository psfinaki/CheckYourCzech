module Task

open Elmish
open Fable.Helpers.React
open Fable.PowerPack
open Fable.Import.React
open Fable.Core.JsInterop

type Task = { 
    Word : string 
    Answers : string[]
}

type Model = {
    Word : string option
    Answers : string[] option
    Input : string
    Result : bool option
}

type Msg = 
    | SetInput of string
    | SubmitAnswer
    | UpdateTask
    | FetchedTask of Task option
    | FetchError of exn

let loadTaskCmd getTask =
    Cmd.ofPromise getTask [] FetchedTask FetchError

let init getTask =
    { Word = None
      Answers = None
      Input = ""
      Result = None },
      loadTaskCmd getTask

let update msg model getTask =
    match msg with
    | SetInput input ->
        { model with Input = input }, Cmd.none
    | SubmitAnswer -> 
        let result = model.Answers |> Option.map (Array.contains model.Input)
        { model with Result = result }, Cmd.none
    | UpdateTask ->
        { model with Word = None; Input = ""; Result = None }, loadTaskCmd getTask
    | FetchedTask task -> 
        { model with 
            Word = task |> Option.map (fun t -> t.Word)
            Answers = task |> Option.map (fun t -> t.Answers)
        }, Cmd.none
    | FetchError _ ->
        model, Cmd.none

let view model dispatch =
    let result = 
        match model.Result with 
        | Some result -> 
            let imageSource = if result then "images/correct.png" else "images/incorrect.png"
            let altText = if result then "Correct" else "Incorrect"
            Markup.icon imageSource 25 altText
        | None ->
            str ""

    let task = 
        match model.Word with
        | Some t -> 
            str t
        | None ->
            let imageSource = "images/loading.gif"
            let altText = "Loading..."
            Markup.icon imageSource 25 altText

    let handleChangeAnswer (event: FormEvent) =
        dispatch (SetInput !!event.target?value)
           
    let handleKeyDown (event: KeyboardEvent) =
        match event.keyCode with
        | Keyboard.Codes.enter ->
            match event.shiftKey with
            | false -> dispatch SubmitAnswer
            | true  -> dispatch UpdateTask
        | _ -> 
            ()

    let handleUpdateClick _ = dispatch UpdateTask
    let handleCheckClick _ = dispatch SubmitAnswer
    
    div []
        [
            div [ Styles.row ] 
                [
                    Markup.label Styles.greyLabel task
                    Markup.input Styles.input model.Input handleChangeAnswer handleKeyDown
                    Markup.label Styles.greyLabel result
                ]

            Markup.emptyLines 2

            div [ Styles.row ]
                [
                    Markup.button (Styles.button "White") handleUpdateClick "Next (⇧ + ⏎)"
                    Markup.space()
                    Markup.button (Styles.button "Lime") handleCheckClick "Check (⏎)"
                ]
        ]
