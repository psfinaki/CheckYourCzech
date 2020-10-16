module Client.Widgets.NumeralRange

open Browser.Types
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props

open Client
open Common.Numerals

let private translateToNumeralRange s =
    let translations = 
        dict [ "From0To20",     Range.From0To20
               "From0To100",    Range.From0To100
               "From0To1000",   Range.From0To1000]

    translations.[s]

[<Literal>]
let NumeralRangeUnset = ""

type Model = {
    NumeralRange : Range option
}

type Msg =
    | SetNumeralRange of Range option

let init() =
    { NumeralRange = None }

let update msg model =
    match msg with
    | SetNumeralRange range ->
        { model with NumeralRange = range }

let view model dispatch =
    let handleChangeNumeralRange (event: Event) =
        let translate = function | NumeralRangeUnset -> None | x -> Some (translateToNumeralRange x)
        dispatch (SetNumeralRange (translate !!event.target?value))

    let selectedValue = model.NumeralRange |> Option.map string |> Option.defaultValue "Any"

    div [ClassName "filter numeral-range-filter"] 
        [
            div [ ClassName "numeral-range-filter-label" ] 
                [
                    label [] [ str "Range" ]
                ]

            div [ ] 
                [
                    Markup.select selectedValue handleChangeNumeralRange [
                        Markup.option NumeralRangeUnset "Any"
                        Markup.option (string Range.From0To20) "0 - 20"
                        Markup.option (string Range.From0To100) "0 - 100"
                        Markup.option (string Range.From0To1000) "0 - 1000"
                    ]
                ]
        ]
