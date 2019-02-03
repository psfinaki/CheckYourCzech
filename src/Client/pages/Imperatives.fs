module Imperatives

open Elmish
open Fable.PowerPack.Fetch
open Thoth.Json

type Model = { 
    Class: Class.Model
    Task : Task.Model
}

type Msg = 
    | Class of Class.Msg
    | Task of Task.Msg
    
let getTask ``class`` =
    let url = 
        match ``class`` with
        | Some c -> "/api/imperatives?class=" + c.ToString()
        | None   -> "/api/imperatives"

    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

let init() =
    let ``class`` = Class.init()
    let task, cmd = Task.init (getTask None)

    { Class = ``class``
      Task = task }, 
    Cmd.map Task cmd

let update msg model =
    match msg with
    | Class msg' ->
        let ``class`` = Class.update msg' model.Class
        { model with Class = ``class`` }, Cmd.none
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task (getTask model.Class.Class)
        { model with Task = task }, Cmd.map Task cmd

let view model dispatch = 
    [ 
        Markup.words 60 "Write imperative for the verb"

        Markup.emptyLines 2

        Class.view (Class >> dispatch)

        Markup.emptyLines 2

        Task.view model.Task (Task >> dispatch)
    ]
