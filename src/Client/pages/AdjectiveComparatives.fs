module AdjectiveComparatives

open Elmish
open Fable.PowerPack.Fetch
open Thoth.Json
open Fable.Helpers.React

type Model = {
    Regularity : Regularity.Model
    Task : Task.Model
}

type Msg = 
    | Regularity of Regularity.Msg
    | Task of Task.Msg

let getTask regularity =
    let url = 
        match regularity with
        | Some r -> "/api/adjectives/comparatives?isRegular=" + r.ToString()
        | None   -> "/api/adjectives/comparatives"

    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

let init() =
    let regularity = Regularity.init()
    let task, cmd = Task.init "Adjective Comparatives" (getTask None)

    { Regularity = regularity
      Task = task },
    Cmd.map Task cmd

let update msg model =
    match msg with
    | Regularity msg' ->
        let regularity = Regularity.update msg' model.Regularity
        { model with Regularity = regularity }, Cmd.none
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task (getTask model.Regularity.Regularity)
        { model with Task = task }, Cmd.map Task cmd

let view model dispatch =
    [ 
        Markup.words 60 "Write comparative for the adjective"

        div [ Styles.middle ]
            [
                Markup.emptyLines 2

                Regularity.view model.Regularity (Regularity >> dispatch)

                Markup.emptyLines 2

                Task.view model.Task (Task >> dispatch)

                Markup.emptyLines 1

                Rule.view Rules.comparatives
            ]

    ]
