module Client.Pages.VerbImperatives

open Elmish
open Fable.React

open Client.Markup
open Client.Styles
open Client.Utils
open Client.Widgets
open Common.GrammarCategories.Verbs

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
    dict [ (VerbClass.E, ["nést"; "číst"; "péct"; "třít"; "brát"; "mazat"])
           (VerbClass.NE, ["tisknout"; "minout"; "začít"])
           (VerbClass.JE, ["krýt"; "kupovat"])
           (VerbClass.Í, ["prosit"; "čistit"; "trpět"; "sázet"])
           (VerbClass.Á, ["dělat"]) ]

let getPatterns ``class`` = patterns.[``class``]

let getTask ``class`` pattern =
    let classQuery = ``class`` |> Option.map (fun c -> $"class={c}")
    let patternQuery = pattern |> Option.map (fun p -> $"pattern={p}")

    let queryString =
        [ classQuery; patternQuery ]
        |> Seq.choose id
        |> String.concat "&"
    
    let url = 
        if queryString = "" 
        then "/api/verbs/imperatives" 
        else $"/api/verbs/imperatives?{queryString}"

    buildFetchTask url

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

let reloadTaskCmd =
    Task.NextTask |> (Cmd.ofMsg >> Cmd.map Task)

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
        }, reloadTaskCmd
    | Pattern msg' ->
        let pattern = Pattern.update msg' model.Pattern
        { model with Pattern = pattern }, reloadTaskCmd
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task (getTask model.Class.Class model.Pattern.SelectedPattern)
        { model with Task = task }, Cmd.map Task cmd

let view model dispatch = 
    [ 
        words "task-heading" "Write imperative for the verb"

        div [ middle ]
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
