﻿module VerbImperatives

open Elmish
open Fable.PowerPack.Fetch
open Thoth.Json
open Fable.Helpers.React
open Fable.Helpers.React.Props

type Model = { 
    FilterBlock : FilterBlock.Types.Model
    Class: Class.Model
    Pattern: Pattern.Model
    Task : Task.Model
}

type Msg = 
    | FilterBlock of FilterBlock.Types.Msg
    | Class of Class.Msg
    | Pattern of Pattern.Msg
    | Task of Task.Msg
    
let patterns =
    dict [ (1, ["nést"; "číst"; "péct"; "třít"; "brát"; "mazat"])
           (2, ["tisknout"; "minout"; "začít"])
           (3, ["krýt"; "kupovat"])
           (4, ["prosit"; "čistit"; "trpět"; "sázet"])
           (5, ["dělat"]) ]

let getPatterns ``class`` = patterns.[``class``]

let getTask ``class`` pattern =
    let classQuery = ``class`` |> Option.map (sprintf "class=%i")
    let patternQuery = pattern |> Option.map (sprintf "pattern=%s")

    let queryString =
        [ classQuery; patternQuery ]
        |> Seq.choose id
        |> String.concat "&"
    
    let url = 
        if queryString = "" 
        then "/api/verbs/imperatives" 
        else sprintf "/api/verbs/imperatives?%s" queryString

    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

let init() =
    let filterBlock = FilterBlock.State.init()
    let ``class`` = Class.init()
    let pattern = Pattern.init None
    let task, cmd = Task.init "Verb Imperatives" (getTask None None)

    { FilterBlock = filterBlock
      Class = ``class``
      Pattern = pattern
      Task = task }, 
    Cmd.map Task cmd

let update msg model =
    match msg with
    | FilterBlock msg' ->
        let filterBlock, cmd = FilterBlock.State.update msg' model.FilterBlock
        { model with FilterBlock = filterBlock }, cmd
    | Class msg' ->
        let ``class`` = Class.update msg' model.Class
        let patterns = ``class``.Class |> Option.map getPatterns
        let pattern = Pattern.update (Pattern.SetPatterns patterns) model.Pattern
        { model with 
            Class = ``class`` 
            Pattern = pattern
        }, Cmd.none
    | Pattern msg' ->
        let pattern = Pattern.update msg' model.Pattern
        { model with Pattern = pattern }, Cmd.none
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task (getTask model.Class.Class model.Pattern.SelectedPattern)
        { model with Task = task }, Cmd.map Task cmd

let view model dispatch = 
    [ 
        Markup.words "task-heading" "Write imperative for the verb"

        div [ Styles.middle ]
            [
                FilterBlock.View.root model.FilterBlock (FilterBlock >> dispatch) 
                    [
                        div [ ]
                            [
                                Class.view model.Class (Class >> dispatch)
                            ]

                        div [ ]
                            [
                                Pattern.view model.Pattern (Pattern >> dispatch)
                            ]
                    ]

                Task.view model.Task (Task >> dispatch)
            ]
    ]
