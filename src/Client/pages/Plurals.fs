module Plurals

open Elmish
open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack
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
    let result = 
        match model.Result with 
        | Some result -> 
            let imageSource = if result then "images/correct.png" else "images/incorrect.png"
            let altText = if result then "Correct" else "Incorrect"
            Markup.icon imageSource 25 altText
        | None ->
            str "-"

    let task = 
        if model.Task <> "" 
        then model.Task 
        else "-"
        |> str

    let handleChangeAnswer (event: FormEvent) =
        dispatch (SetInput !!event.target?value)
        
    let handleChangeGender (event: FormEvent) =
        dispatch (SetGender (translateTo !!event.target?value))
        
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
        Markup.words 60 "Write plural for the word"

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
                            Markup.option (translateFrom MasculineAnimate) "Masculine Animate"
                            Markup.option (translateFrom MasculineInanimate) "Masculine Inanimate"
                            Markup.option (translateFrom Feminine) "Feminine"
                            Markup.option (translateFrom Neuter) "Neuter"
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
                Markup.button (Styles.button "White") handleUpdateClick "Next"
                Markup.space()
                Markup.button (Styles.button "Lime") handleCheckClick "Check"
            ]
    ]