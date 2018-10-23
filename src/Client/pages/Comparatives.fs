module Comparatives

open System
open Elmish
open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack
open Fable.Import.React

// because Fable cannot compile bool.Parse
type Boolean with
    static member FromString = function
        | "True" -> true
        | "False" -> false
        | _ -> invalidOp "Value cannot be converted to boolean."

type Model = {
    Regularity : bool option
    Task : string option
    Input : string
    InputSubmitted: bool
    Result : bool option
}

type Msg = 
    | SetRegularity of bool option
    | SetInput of string
    | SubmitTask
    | UpdateTask
    | FetchedTask of string
    | FetchedAnswer of string[]
    | FetchError of exn

let getTask regularity =
    promise {
        let url = 
            match regularity with
            | Some r -> "/api/comparatives/task?isRegular=" + r.ToString()
            | None   -> "/api/comparatives/task"

        return! Fetch.fetchAs<string> url []
    }

let getAnswer task = 
    promise {
        let url = "/api/comparatives/answer/" + task
        return! Fetch.fetchAs<string[]> url []
    }

[<Literal>]
let RegularityUnset = ""

let loadTaskCmd regularity =
    Cmd.ofPromise getTask regularity FetchedTask FetchError

let loadAnswerCmd task =
    Cmd.ofPromise getAnswer task FetchedAnswer FetchError

let submitTask = function
    | Some task -> loadAnswerCmd task
    | None -> Cmd.none

let init () =
    { Task = None
      Regularity = None
      Input = ""
      InputSubmitted = false
      Result = None },
      loadTaskCmd None

let update msg model =
    match msg with
    | SetRegularity regularity ->
        { model with Regularity = regularity }, Cmd.none
    | SetInput input ->
        { model with Input = input }, Cmd.none
    | SubmitTask ->
        { model with Result = None; InputSubmitted = true }, submitTask model.Task
    | UpdateTask ->
        { model with Task = None; Input = ""; Result = None }, loadTaskCmd model.Regularity
    | FetchedTask task ->
        { model with Task = Some task }, Cmd.none
    | FetchedAnswer answer ->
        let result = answer |> Array.contains model.Input
        { model with Result = Some result; InputSubmitted = false }, Cmd.none
    | FetchError _ ->
        model, Cmd.none

let view model dispatch =
    let result = 
        match model.Result with 
        | Some result -> 
            let imageSource = if result then "images/correct.png" else "images/incorrect.png"
            let altText = if result then "Correct" else "Incorrect"
            Markup.icon imageSource 25 altText
        | None when model.Task.IsSome && model.InputSubmitted ->
            let imageSource = "images/loading.gif"
            let altText = "Loading..."
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
        
    let handleChangeRegularity (event: FormEvent) =
        let translate = function | RegularityUnset -> None | x -> Some (bool.FromString x)
        dispatch (SetRegularity (translate !!event.target?value))

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
        Markup.words 60 "Write comparative for the adjective"

        Markup.emptyLines 3

        div [ Styles.row ] 
            [
                div [ Style [ Height "50%" ] ] 
                    [
                        Markup.label Styles.whiteLabel (str "Regularity") 
                    ]

                div [ Styles.select ] 
                    [
                        Markup.select handleChangeRegularity [
                            Markup.option RegularityUnset "Any"
                            Markup.option "True" "Regular"     // true.ToString()  does not work
                            Markup.option "False" "Exceptions" // false.ToString() does not work
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