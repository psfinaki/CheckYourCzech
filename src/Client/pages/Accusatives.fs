module Accusatives

open Elmish
open Fable.PowerPack.Fetch
open Genders
open Thoth.Json
open Fable.Helpers.React

type Model = {
    Gender : Gender.Model
    Pattern : NounPattern.Model
    Task : Task.Model
}

type Msg = 
    | Gender of Gender.Msg
    | Pattern of NounPattern.Msg
    | Task of Task.Msg

let getTask gender pattern =
    let genderQuery = gender |> Option.map Gender.ToString |> Option.map (sprintf "gender=%s")
    let patternQuery = pattern |> Option.map (sprintf "pattern=%s")

    let queryString =
        [ genderQuery; patternQuery ]
        |> Seq.choose id
        |> String.concat "&"
    
    let url = 
        if queryString = "" 
        then "/api/accusatives" 
        else sprintf "/api/accusatives?%s" queryString

    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

let init () =
    let gender = Gender.init()
    let pattern = NounPattern.init()
    let task, cmd = Task.init (getTask None None)

    { Gender = gender
      Pattern = pattern
      Task = task },
    Cmd.map Task cmd

let update msg model =
    match msg with
    | Gender msg' ->
        let gender = Gender.update msg' model.Gender
        let pattern = NounPattern.update (NounPattern.SetGender gender.Gender) model.Pattern
        { model with 
            Gender = gender 
            Pattern = pattern
        }, Cmd.none
    | Pattern msg' ->
        let pattern = NounPattern.update msg' model.Pattern
        { model with Pattern = pattern }, Cmd.none
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task (getTask model.Gender.Gender model.Pattern.Pattern)
        { model with Task = task }, Cmd.map Task cmd
        
let view model dispatch =    
    [ 
        Markup.words 60 "Write accusative for the word"

        div [ Styles.middle ]
            [
                Markup.emptyLines 2

                div []
                    [
                        div [ Styles.halfParent ]
                            [
                                Gender.view (Gender >> dispatch)
                            ]

                        div [ Styles.halfParent ]
                            [
                                NounPattern.view model.Pattern (Pattern >> dispatch)
                            ]
                    ]

                Markup.emptyLines 2

                Task.view model.Task (Task >> dispatch)
            ]
    ]
