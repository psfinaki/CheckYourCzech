module Imperatives

open Elmish
open Fable.PowerPack.Fetch
open Thoth.Json

type Model = { Task : Task.Model }

type Msg = | Task of Task.Msg

let getTask =
    let url = "/api/imperatives"
    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

let init() =
    let task, cmd = Task.init getTask
    { Task = task }, Cmd.map Task cmd

let update msg model =
    match msg with
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task getTask
        { model with Task = task }, Cmd.map Task cmd

let view model dispatch = 
    [ 
        Markup.words 60 "Write imperative for the verb"

        Markup.emptyLines 8

        Task.view model.Task (Task >> dispatch)
    ]
