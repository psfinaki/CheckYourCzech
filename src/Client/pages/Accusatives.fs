module Accusatives

open Elmish
open Fable.PowerPack.Fetch
open Gender
open Thoth.Json

type Model = {
    GenderModel : Gender.Model
    TaskModel : Task.Model
}

type Msg = 
    | GenderMsg of Gender.Msg
    | TaskMsg of Task.Msg

let getTask gender =
    let url = 
        match gender with
        | Some g -> "/api/accusatives?gender=" + Gender.ToString g
        | None   -> "/api/accusatives"

    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

let init () =
    let genderModel = Gender.init()
    let taskModel, taskCmd = Task.init (getTask None)

    { GenderModel = genderModel
      TaskModel = taskModel },
    Cmd.map TaskMsg taskCmd

let update msg model =
    match msg with
    | GenderMsg msg' ->
        let result = Gender.update msg' model.GenderModel
        { model with GenderModel = result }, Cmd.none
    | TaskMsg msg' ->
        let result, cmd = Task.update msg' model.TaskModel (getTask model.GenderModel.Gender)
        { model with TaskModel = result }, Cmd.map TaskMsg cmd

let view model dispatch =    
    [ 
        Markup.words 60 "Write accusative for the word"

        Markup.emptyLines 3

        Gender.view (GenderMsg >> dispatch)

        Markup.emptyLines 2

        Task.view model.TaskModel (TaskMsg >> dispatch)
    ]
