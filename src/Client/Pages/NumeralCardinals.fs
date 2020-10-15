module Client.Pages.NumeralCardinals

open Elmish
open Fable.React

open Client.Markup
open Client.Styles
open Client.Utils
open Client.Widgets

type Model = {
    FilterBlock : FilterBlock.Types.Model
    NumeralRange : NumeralRange.Model
    Task : Task.Model
}

type Msg = 
    | FilterBlock of FilterBlock.Types.Msg
    | NumeralRange of NumeralRange.Msg
    | Task of Task.Msg


let getTask range =
    let rangeQuery = range |> Option.map (sprintf "range=%A")
    let queryString =
        [ rangeQuery ]
        |> Seq.choose id
        |> String.concat "&"
    
    let url = 
        if queryString = "" 
        then "/api/numerals/cardinals" 
        else sprintf "/api/numerals/cardinals?%s" queryString

    buildFetchTask url

let init () =
    let filterBlock = FilterBlock.State.init()
    let range = NumeralRange.init()
    let task, cmd = Task.init "Numerals Cardinals" (getTask None)

    { FilterBlock = filterBlock
      NumeralRange = range
      Task = task },
    Cmd.map Task cmd

let reloadTaskCmd =
    Task.NextTask |> (Cmd.ofMsg >> Cmd.map Task)

let update msg model =
    match msg with
    | FilterBlock msg' ->
        let filterBlock, cmd = FilterBlock.State.update msg' model.FilterBlock
        { model with FilterBlock = filterBlock }, cmd
    | NumeralRange msg' ->
        let range = NumeralRange.update msg' model.NumeralRange
        { model with 
            NumeralRange = range
        }, reloadTaskCmd
    | Task msg' ->
        let range = model.NumeralRange.NumeralRange
        let task, cmd = Task.update msg' model.Task (getTask range)
        { model with Task = task }, Cmd.map Task cmd
        
let view model dispatch =    
    [ 
        words "task-heading" "Write written form for a cardinal (e.g. 42 -> čtyřicet dva)"

        div [ middle ]
            [
                FilterBlock.View.root model.FilterBlock (FilterBlock >> dispatch) 
                    [
                        NumeralRange.view model.NumeralRange (NumeralRange >> dispatch)
                    ]

                Task.view model.Task (Task >> dispatch)
            ]
    ]
