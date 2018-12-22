﻿module Accusatives

open Elmish
open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack
open Fable.PowerPack.Fetch
open Fable.Import.React
open Gender
open Thoth.Json
open Tasks

type Model = {
    Gender : Gender option
    Task : string option
    Answers: string[] option
    Input : string
    Result : bool option
}

type Msg = 
    | SetGender of Gender option
    | SetInput of string
    | SubmitTask
    | UpdateTask
    | FetchedTask of AccusativesTask option
    | FetchError of exn

let getTask gender =
    let url = 
        match gender with
        | Some g -> "/api/accusatives?gender=" + Gender.ToString g
        | None   -> "/api/accusatives"
    fetchAs<AccusativesTask option> url (Decode.Auto.generateDecoder())

[<Literal>]
let GenderUnset = ""

let loadTaskCmd gender =
    Cmd.ofPromise (getTask gender) [] FetchedTask FetchError

let init () =
    { Gender = None
      Task = None
      Answers = None
      Input = ""
      Result = None },
      loadTaskCmd None

let update msg model =
    match msg with
    | SetGender gender ->
        { model with Gender = gender }, Cmd.none
    | SetInput input ->
        { model with Input = input }, Cmd.none
    | SubmitTask ->
        let result = model.Answers |> Option.map (Array.contains model.Input)
        { model with Result = result }, Cmd.none
    | UpdateTask ->
        { model with Task = None; Input = ""; Result = None }, loadTaskCmd model.Gender
    | FetchedTask task ->
        { model with 
            Task = task |> Option.map (fun t -> t.Singulars |> Seq.random)
            Answers = task |> Option.map (fun t -> t.Accusatives)    
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
        
    let handleChangeGender (event: FormEvent) =
        let translate = function | GenderUnset -> None | x -> Some (Gender.FromString x)
        dispatch (SetGender (translate !!event.target?value))
        
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
        Markup.words 60 "Write accusative for the word"

        Markup.emptyLines 3

        div [ Styles.row ] 
            [
                div [ Style [ Height "50%" ] ] 
                    [
                        Markup.label Styles.whiteLabel (str "Gender") 
                    ]

                div [ Styles.select ] 
                    [
                        Markup.select handleChangeGender [
                            Markup.option GenderUnset "Any"
                            Markup.option (Gender.ToString MasculineAnimate) "Masculine Animate"
                            Markup.option (Gender.ToString MasculineInanimate) "Masculine Inanimate"
                            Markup.option (Gender.ToString Feminine) "Feminine"
                            Markup.option (Gender.ToString Neuter) "Neuter"
                        ]
                    ]
            ]

        Markup.emptyLines 2

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