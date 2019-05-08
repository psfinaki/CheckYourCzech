module NounPlurals

open Elmish
open Fable.PowerPack.Fetch
open Thoth.Json
open Fable.Helpers.React
open Genders

type Model = {
    Gender : Gender.Model
    Pattern : Pattern.Model
    Task : Task.Model
}

type Msg = 
    | Gender of Gender.Msg
    | Pattern of Pattern.Msg
    | Task of Task.Msg

let patterns =
    dict [ (MasculineAnimate, ["pan"; "muž"; "předseda"; "soudce"])
           (MasculineInanimate, ["hrad"; "stroj"])
           (Feminine, ["žena"; "růže"; "píseň"; "kost"])
           (Neuter, ["město"; "kuře"; "moře"; "stavení"]) ]

let getPatterns gender = patterns.[gender]

let getTask gender pattern =
    let genderQuery = gender |> Option.map string |> Option.map (sprintf "gender=%s")
    let patternQuery = pattern |> Option.map (sprintf "pattern=%s")

    let queryString =
        [ genderQuery; patternQuery ]
        |> Seq.choose id
        |> String.concat "&"
    
    let url = 
        if queryString = "" 
        then "/api/nouns/plurals" 
        else sprintf "/api/nouns/plurals?%s" queryString

    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

let init () =
    let gender = Gender.init()
    let pattern = Pattern.init None
    let task, cmd = Task.init (getTask None None)

    { Gender = gender
      Pattern = pattern
      Task = task },
    Cmd.map Task cmd

let update msg model =
    match msg with
    | Gender msg' ->
        let gender = Gender.update msg' model.Gender
        let patterns = gender.Gender |> Option.map getPatterns
        let pattern = Pattern.update (Pattern.SetPatterns patterns) model.Pattern
        { model with 
            Gender = gender 
            Pattern = pattern
        }, Cmd.none
    | Pattern msg' ->
        let pattern = Pattern.update msg' model.Pattern
        { model with Pattern = pattern }, Cmd.none
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task (getTask model.Gender.Gender model.Pattern.SelectedPattern)
        { model with Task = task }, Cmd.map Task cmd

let view model dispatch =
    [ 
        Markup.words 60 "Write plural for the word"

        div [ Styles.middle ] 
            [
                Markup.emptyLines 2

                div []
                    [
                        div [ Styles.halfParent ]
                            [
                                Gender.view model.Gender (Gender >> dispatch)
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
