module Imperatives

open Elmish
open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.PowerPack
open Fable.PowerPack.Fetch
open Fable.Import.React
open Thoth.Json
open Tasks

type Model = {
    Task : string option
    Answers: string[] option
    Input : string
    Result : bool option
}

type Msg = 
    | SetInput of string
    | SubmitTask
    | UpdateTask
    | FetchedTask of ImperativesTask option
    | FetchError of exn

let getTask =
    let url = "/api/imperatives"
    fetchAs<ImperativesTask option> url (Decode.Auto.generateDecoder())

let loadTaskCmd() =
    Cmd.ofPromise getTask [] FetchedTask FetchError
    
let init () =
    { Task = None
      Answers = None
      Input = ""
      Result = None },
      loadTaskCmd() 

let update msg model =
    match msg with
    | SetInput input ->
        { model with Input = input }, Cmd.none
    | SubmitTask -> 
        let result = model.Answers |> Option.map (Array.contains model.Input)
        { model with Result = result }, Cmd.none
    | UpdateTask ->
        { model with Task = None; Input = ""; Result = None }, loadTaskCmd()
    | FetchedTask task -> 
        { model with 
            Task = task |> Option.map (fun t -> t.Indicative)
            Answers = task |> Option.map (fun t -> t.Imperatives)
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
        match model.Task with
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
            | false -> dispatch SubmitTask
            | true  -> dispatch UpdateTask
        | _ -> 
            ()

    let handleUpdateClick _ = dispatch UpdateTask
    let handleCheckClick _ = dispatch SubmitTask
    
    [ 
        Markup.words 60 "Write imperative for the verb"

        Markup.emptyLines 8

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
