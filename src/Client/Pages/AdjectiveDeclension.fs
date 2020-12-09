module Client.Pages.AdjectiveDeclension

open Elmish
open Fable.React

open Client.Markup
open Client.Styles
open Client.Utils
open Client.Widgets

type Model = {
    FilterBlock : FilterBlock.Types.Model
    Number : Number.Model
    Case : Case.Model
    Task : Task.Model
}

type Msg = 
    | FilterBlock of FilterBlock.Types.Msg
    | Number of Number.Msg
    | Case of Case.Msg
    | Task of Task.Msg

let getTask number case =
    let numberQuery = number |> Option.map (fun n -> $"number={n}")
    let caseQuery = case |> Option.map (fun c -> $"case={c}")

    let queryString =
        [ numberQuery; caseQuery ]
        |> Seq.choose id
        |> String.concat "&"
    
    let url = 
        if queryString = "" 
        then "/api/adjectives/declension" 
        else $"/api/adjectives/declension?{queryString}"

    buildFetchTask url

let init () =
    let filterBlock = FilterBlock.State.init()
    let number = Number.init()
    let case = Case.init()
    let task, cmd = Task.init "Adjective Declension" (getTask None None)

    { FilterBlock = filterBlock
      Number = number
      Case = case
      Task = task },
    Cmd.map Task cmd

let reloadTaskCmd =
    Task.NextTask |> (Cmd.ofMsg >> Cmd.map Task)

let update msg model =
    match msg with
    | FilterBlock msg' ->
        let filterBlock, cmd = FilterBlock.State.update msg' model.FilterBlock
        { model with FilterBlock = filterBlock }, cmd
    | Number msg' ->
        let number = Number.update msg' model.Number
        { model with Number = number }, reloadTaskCmd
    | Case msg' ->
        let case = Case.update msg' model.Case
        { model with Case = case }, reloadTaskCmd
    | Task msg' ->
        let number = model.Number.Number
        let case = model.Case.Case
        let task, cmd = Task.update msg' model.Task (getTask number case)
        { model with Task = task }, Cmd.map Task cmd
        
let view model dispatch =    
    [ 
        words "task-heading" "Write corresponding declension for the adjective"

        div [ middle ]
            [
                FilterBlock.View.root model.FilterBlock (FilterBlock >> dispatch) 
                    [
                        div [ ]
                            [
                                Number.view model.Number (Number >> dispatch)
                            ]

                        div [ ]
                            [
                                Case.view model.Case (Case >> dispatch)
                            ]
                    ]

                Task.view model.Task (Task >> dispatch)
            ]
    ]
