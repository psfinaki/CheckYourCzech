module Class

open System
open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Import.React

[<Literal>]
let ClassUnset = ""

type Model = { 
    Class : int option
}

type Msg =
    | SetClass of int option

let init() =
    { Class = None }

let update msg model =
    match msg with
    | SetClass ``class`` ->
        { model with Class = ``class`` }

let view model dispatch =
    let handleChangeClass (event: FormEvent) =
        let translate = function | ClassUnset -> None | x -> Some (Int32.Parse x)
        dispatch (SetClass (translate !!event.target?value))

    let selectedValue = model.Class |> Option.map string |> Option.defaultValue "Any"

    div [ ClassName "class-filter" ] 
        [
            div [ ClassName "class-filter-label" ] 
                [
                    Markup.label Styles.whiteLabel (str "Class") 
                ]

            div [ Styles.select ] 
                [
                    Markup.select selectedValue handleChangeClass [
                        Markup.option ClassUnset "Any"
                        Markup.option 1 "1"
                        Markup.option 2 "2"
                        Markup.option 3 "3"
                        Markup.option 4 "4"
                        Markup.option 5 "5"
                    ]
                ]
        ]
