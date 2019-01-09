module Accusatives

open Elmish
open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch
open Fable.Import.React
open Gender
open Thoth.Json

type Model = {
    Gender : Gender option
    TaskModel : Task.Model
}

type Msg = 
    | SetGender of Gender option
    | TaskMsg of Task.Msg

let getTask gender =
    let url = 
        match gender with
        | Some g -> "/api/accusatives?gender=" + Gender.ToString g
        | None   -> "/api/accusatives"

    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

[<Literal>]
let GenderUnset = ""

let init () =
    let taskModel, taskCmd = Task.init (getTask None)
    { Gender = None
      TaskModel = taskModel },
      Cmd.map TaskMsg taskCmd

let update msg model =
    match msg with
    | SetGender gender ->
        { model with Gender = gender }, Cmd.none
    | TaskMsg msg' ->
        let result, cmd = Task.update msg' model.TaskModel (getTask model.Gender)
        { model with TaskModel = result }, Cmd.map TaskMsg cmd

let view model dispatch =
    let handleChangeGender (event: FormEvent) =
        let translate = function | GenderUnset -> None | x -> Some (Gender.FromString x)
        dispatch (SetGender (translate !!event.target?value))
        
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

        Task.view model.TaskModel (TaskMsg >> dispatch)
    ]
