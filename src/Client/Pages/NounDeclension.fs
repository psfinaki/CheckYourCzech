module Client.Pages.NounDeclension

open Elmish
open Fable.React

open Common.GrammarCategories
open Client.Markup
open Client.Styles
open Client.Utils
open Client.Widgets

type Model = {
    FilterBlock : FilterBlock.Types.Model
    Gender : Gender.Model
    Pattern : Pattern.Model
    Number : Number.Model
    Case : Case.Model
    Task : Task.Model
}

type Msg = 
    | FilterBlock of FilterBlock.Types.Msg
    | Gender of Gender.Msg
    | Pattern of Pattern.Msg
    | Number of Number.Msg
    | Case of Case.Msg
    | Task of Task.Msg

let patterns =
    dict [ (MasculineAnimate, ["pán"; "muž"; "předseda"; "soudce"])
           (MasculineInanimate, ["hrad"; "stroj"; "rytmus"])
           (Feminine, ["žena"; "růže"; "píseň"; "kost"])
           (Neuter, ["město"; "kuře"; "moře"; "stavení"; "drama"; "muzeum" ]) ]

let getPatterns gender = patterns.[gender]

let getTask gender pattern number case =
    let genderQuery = gender |> Option.map (sprintf "gender=%A")
    let patternQuery = pattern |> Option.map (sprintf "pattern=%s")
    let numberQuery = number |> Option.map (sprintf "number=%A")
    let caseQuery = case |> Option.map (sprintf "case=%A")

    let queryString =
        [ genderQuery; patternQuery; numberQuery; caseQuery ]
        |> Seq.choose id
        |> String.concat "&"
    
    let url = 
        if queryString = "" 
        then "/api/nouns/declension" 
        else sprintf "/api/nouns/declension?%s" queryString

    buildFetchTask url

let init () =
    let filterBlock = FilterBlock.State.init()
    let gender = Gender.init()
    let pattern = Pattern.init None
    let number = Number.init()
    let case = Case.init()
    let task, cmd = Task.init "Noun Declension" (getTask None None None None)

    { FilterBlock = filterBlock
      Gender = gender
      Pattern = pattern
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
    | Number msg' ->
        let number = Number.update msg' model.Number
        { model with Number = number }, reloadTaskCmd
    | Case msg' ->
        let case = Case.update msg' model.Case
        { model with Case = case }, reloadTaskCmd
    | Task msg' ->
        let gender = model.Gender.Gender
        let pattern = model.Pattern.SelectedPattern
        let number = model.Number.Number
        let case = model.Case.Case
        let task, cmd = Task.update msg' model.Task (getTask gender pattern number case)
        { model with Task = task }, Cmd.map Task cmd
        
let view model dispatch =    
    [ 
        words "task-heading" "Write corresponding declension for the noun"

        div [ middle ]
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
