module Comparatives

open System
open Elmish
open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch
open Fable.Import.React
open Thoth.Json
open Tasks 

// because Fable cannot compile bool.Parse
type Boolean with
    static member FromString = function
        | "True" -> true
        | "False" -> false
        | _ -> invalidOp "Value cannot be converted to boolean."

type Model = {
    Regularity : bool option
    TaskModel : Task.Model
}

type Msg = 
    | SetRegularity of bool option
    | TaskMsg of Task.Msg

let getTask regularity =
    let url = 
        match regularity with
        | Some r -> "/api/comparatives?isRegular=" + r.ToString()
        | None   -> "/api/comparatives"

    fetchAs<CommonTask option> url (Decode.Auto.generateDecoder())

[<Literal>]
let RegularityUnset = ""

let init() =
    let taskModel, taskCmd = Task.init (getTask None)
    { Regularity = None
      TaskModel = taskModel },
      Cmd.map TaskMsg taskCmd

let update msg model =
    match msg with
    | SetRegularity regularity ->
        { model with Regularity = regularity }, Cmd.none
    | TaskMsg msg' ->
        let result, cmd = Task.update msg' model.TaskModel (getTask model.Regularity)
        { model with TaskModel = result }, Cmd.map TaskMsg cmd

let view model dispatch =
    let handleChangeRegularity (event: FormEvent) =
        let translate = function | RegularityUnset -> None | x -> Some (bool.FromString x)
        dispatch (SetRegularity (translate !!event.target?value))
    
    [ 
        Markup.words 60 "Write comparative for the adjective"

        Markup.emptyLines 2

        div [ Styles.row ] 
            [
                div [ Style [ Height "50%" ] ] 
                    [
                        Markup.label Styles.whiteLabel (str "Regularity") 
                    ]

                div [ Styles.select ] 
                    [
                        Markup.select handleChangeRegularity [
                            Markup.option RegularityUnset "Any"
                            Markup.option "True" "Regular"     // true.ToString()  does not work
                            Markup.option "False" "Exceptions" // false.ToString() does not work
                        ]
                    ]
            ]

        Markup.emptyLines 2

        div [] (Task.view model.TaskModel (TaskMsg >> dispatch))

        Markup.emptyLines 1

        div [ Styles.row ]
            [
                Markup.toggleLink "rules" Rules.comparatives
            ]
    ]
