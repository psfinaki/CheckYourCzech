module Client.Pages.NounAccusatives

open Elmish
open Thoth.Fetch
open Thoth.Json
open Fable.React

open Common.GrammarCategories
open Client
open Client.Widgets

type Model = {
    FilterBlock : FilterBlock.Types.Model
    Gender : Gender.Model
    Pattern : Pattern.Model
    Task : Task.Model
}

type Msg = 
    | FilterBlock of FilterBlock.Types.Msg
    | Gender of Gender.Msg
    | Pattern of Pattern.Msg
    | Task of Task.Msg

let patterns =
    dict [ (MasculineAnimate, ["pán"; "muž"; "předseda"; "soudce"])
           (MasculineInanimate, ["hrad"; "stroj"; "rytmus"])
           (Feminine, ["žena"; "růže"; "píseň"; "kost"])
           (Neuter, ["město"; "kuře"; "moře"; "stavení"; "drama"; "muzeum" ]) ]

let getPatterns gender = patterns.[gender]

let getTask gender pattern =
    let genderQuery = gender |> Option.map (sprintf "gender=%A")
    let patternQuery = pattern |> Option.map (sprintf "pattern=%s")

    let queryString =
        [ genderQuery; patternQuery ]
        |> Seq.choose id
        |> String.concat "&"
    
    let url = 
        if queryString = "" 
        then "/api/nouns/accusatives" 
        else sprintf "/api/nouns/accusatives?%s" queryString

    Fetch.fetchAs(url, Decode.Auto.generateDecoder<Task.Task option>())

let init () =
    let filterBlock = FilterBlock.State.init()
    let gender = Gender.init()
    let pattern = Pattern.init None
    let task, cmd = Task.init "Noun Accusatives" (getTask None None)

    { FilterBlock = filterBlock
      Gender = gender
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
    | Gender msg' ->
        let gender = Gender.update msg' model.Gender
        let patterns = gender.Gender |> Option.map getPatterns
        let pattern = Pattern.update (Pattern.SetPatterns patterns) model.Pattern
        { model with 
            Gender = gender 
            Pattern = pattern
        }, reloadTaskCmd
    | Pattern msg' ->
        let pattern = Pattern.update msg' model.Pattern
        { model with Pattern = pattern }, reloadTaskCmd
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task (getTask model.Gender.Gender model.Pattern.SelectedPattern)
        { model with Task = task }, Cmd.map Task cmd
        
let view model dispatch =    
    [ 
        Markup.words "task-heading" "Write accusative for the word"

        div [ Styles.middle ]
            [
                FilterBlock.View.root model.FilterBlock (FilterBlock >> dispatch) 
                    [
                        div [  ]
                            [
                                Gender.view model.Gender (Gender >> dispatch)
                            ]

                        div [ ]
                            [
                                Pattern.view model.Pattern (Pattern >> dispatch)
                            ]
                    ]

                Task.view model.Task (Task >> dispatch)
            ]
    ]
