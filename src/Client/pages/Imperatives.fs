module Imperatives

open Elmish
open Fable.PowerPack.Fetch
open Thoth.Json
open Fable.Helpers.React

type Model = { 
    Class: Class.Model
    Pattern: Pattern.Model
    Task : Task.Model
}

type Msg = 
    | Class of Class.Msg
    | Pattern of Pattern.Msg
    | Task of Task.Msg
    
let getTask ``class`` pattern =
    let classQuery = ``class`` |> Option.map (sprintf "class=%i")
    let patternQuery = pattern |> Option.map (sprintf "pattern=%s")

    let queryString =
        [ classQuery; patternQuery ]
        |> Seq.choose id
        |> String.concat "&"
    
    let url = 
        if queryString = "" 
        then "/api/imperatives" 
        else sprintf "/api/imperatives?%s" queryString

    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

let init() =
    let ``class`` = Class.init()
    let pattern = Pattern.init()
    let task, cmd = Task.init (getTask None None)

    { Class = ``class``
      Pattern = pattern
      Task = task }, 
    Cmd.map Task cmd

let update msg model =
    match msg with
    | Class msg' ->
        let ``class`` = Class.update msg' model.Class
        let pattern = Pattern.update (Pattern.SetClass ``class``.Class) model.Pattern
        { model with 
            Class = ``class`` 
            Pattern = pattern
        }, Cmd.none
    | Pattern msg' ->
        let pattern = Pattern.update msg' model.Pattern
        { model with Pattern = pattern }, Cmd.none
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task (getTask model.Class.Class model.Pattern.Pattern)
        { model with Task = task }, Cmd.map Task cmd

let view model dispatch = 
    [ 
        Markup.words 60 "Write imperative for the verb"

        div [ Styles.middle ]
            [
                Markup.emptyLines 2

                div []
                    [
                        div [ Styles.halfParent ]
                            [
                                Class.view (Class >> dispatch)
                            ]

                        div [ Styles.halfParent ]
                            [
                                Pattern.view model.Pattern (Pattern >> dispatch)
                            ]
                    ]


                Markup.emptyLines 2

                Task.view model.Task (Task >> dispatch)
            ]
    ]
