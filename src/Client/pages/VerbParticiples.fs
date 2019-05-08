module VerbParticiples

open Elmish
open Fable.PowerPack.Fetch
open Thoth.Json
open Fable.Helpers.React

type Model = {
    Pattern : Pattern.Model
    Regularity : Regularity.Model
    Task : Task.Model
}

type Msg = 
    | Pattern of Pattern.Msg
    | Regularity of Regularity.Msg
    | Task of Task.Msg

let patterns = ["Minout"; "Tisknout"; "Common"]

let getTask pattern regularity =
    let patternQuery = pattern |> Option.map (sprintf "pattern=%s")
    let regularityQuery = regularity |> Option.map string |> Option.map ((+) "isRegular=")

    let queryString =
        [ patternQuery; regularityQuery ]
        |> Seq.choose id
        |> String.concat "&"
    
    let url = 
        if queryString = "" 
        then "/api/verbs/participles" 
        else sprintf "/api/verbs/participles?%s" queryString

    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

let init() =
    let pattern = Pattern.init (Some patterns)
    let regularity = Regularity.init()
    let task, cmd = Task.init (getTask None None)

    { Pattern = pattern
      Regularity = regularity
      Task = task },
    Cmd.map Task cmd

let update msg model =
    match msg with
    | Pattern msg' ->
        let pattern = Pattern.update msg' model.Pattern
        let regularity = Regularity.init()
        { model with 
            Pattern = pattern 
            Regularity = regularity
        }, Cmd.none
    | Regularity msg' ->
        let regularity = Regularity.update msg' model.Regularity
        { model with Regularity = regularity }, Cmd.none
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task (getTask model.Pattern.SelectedPattern model.Regularity.Regularity)
        { model with Task = task }, Cmd.map Task cmd

let view model dispatch =          
    [ 
        Markup.words 60 "Write active participle for the verb"

        div [ Styles.middle ]
            [
                Markup.emptyLines 2
                
                div []
                    [
                        div [ Styles.halfParent ]
                            [
                                Pattern.view model.Pattern (Pattern >> dispatch)
                            ]

                        div [ Styles.halfParent ]
                            [
                                Regularity.view model.Regularity (Regularity >> dispatch)
                            ]
                    ]

                Markup.emptyLines 2

                Task.view model.Task (Task >> dispatch)

                Markup.emptyLines 1

                Rule.view Rules.participles
            ]
    ]
