module Client.Widgets.Number

open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Import.React

open Client
open Common.GrammarCategories
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
    let handleChangeNumber (event: FormEvent) =
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
                        Markup.option (string Singular) "Singular"
                        Markup.option (string Plural) "Plural"
                    ]
                ]
        ]
