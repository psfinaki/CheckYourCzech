module Client.Pages.AdjectiveComparatives

open Elmish
open Fable.React

open Client
open Client.Views
open Client.Widgets
open Client.Utils

type Model = {
    FilterBlock : FilterBlock.Types.Model
    Regularity : Regularity.Model
    Task : Task.Model
}

type Msg = 
    | FilterBlock of FilterBlock.Types.Msg
    | Regularity of Regularity.Msg
    | Task of Task.Msg

let getTask regularity =
    let url = 
        match regularity with
        | Some r -> "/api/adjectives/comparatives?isRegular=" + r.ToString()
        | None   -> "/api/adjectives/comparatives"

    buildFetchTask url

let init() =
    let filterBlock = FilterBlock.State.init()
    let regularity = Regularity.init()
    let task, cmd = Task.init "Adjective Comparatives" (getTask None)

    { FilterBlock = filterBlock
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
    | Regularity msg' ->
        let regularity = Regularity.update msg' model.Regularity
        { model with Regularity = regularity }, reloadTaskCmd
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task (getTask model.Regularity.Regularity)
        { model with Task = task }, Cmd.map Task cmd

let view model dispatch =
    [ 
        Markup.words "task-heading" "Write comparative for the adjective"

        div [ Styles.middle ]
            [
                FilterBlock.View.root model.FilterBlock (FilterBlock >> dispatch) 
                    [
                        Regularity.view model.Regularity (Regularity >> dispatch)
                    ]

                Task.view model.Task (Task >> dispatch)

                Rule.view Rules.comparatives
            ]

    ]
