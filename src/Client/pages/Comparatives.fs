module Comparatives

open Elmish
open Fable.PowerPack.Fetch
open Thoth.Json

type Model = {
    RegularityModel : Regularity.Model
    TaskModel : Task.Model
}

type Msg = 
    | RegularityMsg of Regularity.Msg
    | TaskMsg of Task.Msg

let getTask regularity =
    let url = 
        match regularity with
        | Some r -> "/api/comparatives?isRegular=" + r.ToString()
        | None   -> "/api/comparatives"

    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

let init() =
    let regularityModel = Regularity.init()
    let taskModel, taskCmd = Task.init (getTask None)

    { RegularityModel = regularityModel
      TaskModel = taskModel },
    Cmd.map TaskMsg taskCmd

let update msg model =
    match msg with
    | RegularityMsg msg' ->
        let result = Regularity.update msg' model.RegularityModel
        { model with RegularityModel = result }, Cmd.none
    | TaskMsg msg' ->
        let result, cmd = Task.update msg' model.TaskModel (getTask model.RegularityModel.Regularity)
        { model with TaskModel = result }, Cmd.map TaskMsg cmd

let view model dispatch =
    [ 
        Markup.words 60 "Write comparative for the adjective"

        Markup.emptyLines 2

        Regularity.view (RegularityMsg >> dispatch)

        Markup.emptyLines 2

        Task.view model.TaskModel (TaskMsg >> dispatch)

        Markup.emptyLines 1

        Rule.view Rules.comparatives
    ]
