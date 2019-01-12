module Plurals

open Elmish
open Fable.PowerPack.Fetch
open Genders
open Thoth.Json

type Model = {
    Gender : Gender.Model
    Task : Task.Model
}

type Msg = 
    | Gender of Gender.Msg
    | Task of Task.Msg

let getTask gender =
    let url = 
        match gender with
        | Some g -> "/api/plurals?gender=" + Gender.ToString g
        | None   -> "/api/plurals"
    fetchAs<Task.Task option> url (Decode.Auto.generateDecoder())

let init () =
    let gender = Gender.init()
    let task, cmd = Task.init (getTask None)

    { Gender = gender
      Task = task },
    Cmd.map Task cmd

let update msg model =
    match msg with
    | Gender msg' ->
        let gender = Gender.update msg' model.Gender
        { model with Gender = gender }, Cmd.none
    | Task msg' ->
        let task, cmd = Task.update msg' model.Task (getTask model.Gender.Gender)
        { model with Task = task }, Cmd.map Task cmd

let view model dispatch =
    [ 
        Markup.words 60 "Write plural for the word"

        Markup.emptyLines 3

        Gender.view (Gender >> dispatch)
       
        Markup.emptyLines 2

        Task.view model.Task (Task >> dispatch)
    ]
