module AdjectivePlurals

open Elmish
open Fable.PowerPack.Fetch
open Thoth.Json
open Fable.Helpers.React

type Model = {
    Task : Task.Model
}

type Msg = 
    | Task of Task.Msg

let getTask() =
    let url = "/api/adjectives/plurals"
    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

let init() =
    let task, cmd = Task.init (getTask())

    { Task = task },
    Cmd.map Task cmd

let update msg model =
    match msg with
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task (getTask())
        { model with Task = task }, Cmd.map Task cmd

let view model dispatch =
    [
        Markup.words 60 "Write plural for the adjective"

        div [ Styles.middle ]
            [
                Markup.emptyLines 7

                Task.view model.Task (Task >> dispatch)
            ]
    ]
