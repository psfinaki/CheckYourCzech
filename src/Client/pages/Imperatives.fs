module Imperatives

open Elmish
open Fable.Helpers.React
open Fable.PowerPack.Fetch
open Thoth.Json

type Model = { TaskModel : Task.Model }

type Msg = | TaskMsg of Task.Msg

let getTask =
    let url = "/api/imperatives"
    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

let init() =
    let model, cmd = Task.init getTask
    { TaskModel = model }, Cmd.batch [ Cmd.map TaskMsg cmd ]

let update msg model =
    match msg with
    | TaskMsg msg' ->
        let result, cmd = Task.update msg' model.TaskModel getTask
        { model with TaskModel = result }, Cmd.map TaskMsg cmd

let view model dispatch = 
    [ 
        Markup.words 60 "Write imperative for the verb"

        Markup.emptyLines 8

        Task.view model.TaskModel (TaskMsg >> dispatch)
    ]

