module Client.Widgets.Number

open Browser.Types
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props

open Client
open Common.GrammarCategories.Common
open Common.Utils

[<Literal>]
let NumberUnset = ""

type Model = {
    Number : Number option
}

type Msg =
    | SetNumber of Number option

let init() =
    { Number = None }

let update msg model =
    match msg with
    | SetNumber number ->
        { model with Number = number }

let view model dispatch =
    let handleChangeNumber (event: Event) =
        let translate = function | NumberUnset -> None | x -> Some (parseUnionCase<Number> x)
        dispatch (SetNumber (translate !!event.target?value))

    let selectedValue = model.Number |> Option.map string |> Option.defaultValue "Any"

    div [ClassName "number-filter"] 
        [
            div [ ClassName "number`-filter-label" ] 
                [
                    label [] [ str "Number" ]
                ]

            div [ ] 
                [
                    Markup.select selectedValue handleChangeNumber [
                        Markup.option NumberUnset "Any"
                        Markup.option (string Number.Singular) "Singular"
                        Markup.option (string Number.Plural) "Plural"
                    ]
                ]
        ]
