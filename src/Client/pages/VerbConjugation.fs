module Client.Pages.VerbConjugation

open Elmish
open Fable.PowerPack.Fetch
open Thoth.Json
open Fable.Helpers.React

open Client.Markup
open Client.Styles
open Client.Widgets

type Model = {
    FilterBlock : FilterBlock.Types.Model
    Pattern : Pattern.Model
    Regularity : Regularity.Model
    Task : Task.Model
}

type Msg = 
    | FilterBlock of FilterBlock.Types.Msg
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
        then "/api/verbs/conjugation" 
        else sprintf "/api/verbs/conjugation?%s" queryString

    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

let init() =
    let filterBlock = FilterBlock.State.init()
    let pattern = Pattern.init (Some patterns)
    let regularity = Regularity.init()
    let task, cmd = Task.init "Verb Conjugation" (getTask None None)

    { FilterBlock = filterBlock
      Pattern = pattern
      Regularity = regularity
      Task = task },
    Cmd.map Task cmd

let reloadTaskCmd =
    Task.NextTask |> (Cmd.ofMsg >> Cmd.map Task)

let update msg model =
    match msg with
    | FilterBlock msg' ->
        let filterBlock, cmd = FilterBlock.State.update msg' model.FilterBlock
        { model with FilterBlock = filterBlock }, cmd
    | Pattern msg' ->
        let pattern = Pattern.update msg' model.Pattern
        let regularity = Regularity.init()
        { model with 
            Pattern = pattern 
            Regularity = regularity
        }, reloadTaskCmd
    | Regularity msg' ->
        let regularity = Regularity.update msg' model.Regularity
        { model with Regularity = regularity }, reloadTaskCmd
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task (getTask model.Pattern.SelectedPattern model.Regularity.Regularity)
        { model with Task = task }, Cmd.map Task cmd

let view model dispatch =          
    [ 
        words "task-heading" "Write corresponding conjugation for the verb"

        div [ middle ]
            [
                FilterBlock.View.root model.FilterBlock (FilterBlock >> dispatch) 
                    [
                        div [ ]
                            [
                                Pattern.view model.Pattern (Pattern >> dispatch)
                            ]

                        div [ ]
                            [
                                Regularity.view model.Regularity (Regularity >> dispatch)
                            ]
                    ]
                    
                Task.view model.Task (Task >> dispatch)
            ]
    ]
