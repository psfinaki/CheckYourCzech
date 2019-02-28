module Regularity

open System
open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Import.React

// because Fable cannot compile bool.Parse
type Boolean with
    static member FromString = function
        | "True" -> true
        | "False" -> false
        | _ -> invalidOp "Value cannot be converted to boolean."

[<Literal>]
let RegularityUnset = ""

type Model = { 
    Regularity : bool option
}

type Msg =
    | SetRegularity of bool option

let init() =
    { Regularity = None }

let update msg model =
    match msg with
    | SetRegularity regularity ->
        { model with Regularity = regularity }

let view dispatch =
    let handleChangeRegularity (event: FormEvent) =
        let translate = function | RegularityUnset -> None | x -> Some (bool.FromString x)
        dispatch (SetRegularity (translate !!event.target?value))

    div []
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
