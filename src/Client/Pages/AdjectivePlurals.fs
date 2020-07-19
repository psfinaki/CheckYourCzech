module Client.Pages.AdjectivePlurals

open Elmish
open Fable.React

open Client
open Client.Utils
open Client.Widgets

type Model = {
    Task : Task.Model
}

type Msg = 
    | Task of Task.Msg

let getTask() =
    let url = "/api/adjectives/plurals"
    buildFetchTask url

let init() =
    let task, cmd = Task.init "Adjective Plurals" (getTask())

    { Task = task },
    Cmd.map Task cmd

let update msg model =
    match msg with
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task (getTask())
        { model with Task = task }, Cmd.map Task cmd

let view model dispatch =
    [
        Markup.words "task-heading" "Write plural for the adjective"

        div [ Styles.middle ]
            [
                Task.view model.Task (Task >> dispatch)
            ]
    ]
