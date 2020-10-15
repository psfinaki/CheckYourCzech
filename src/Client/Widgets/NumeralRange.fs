module Client.Widgets.NumeralRange

open Browser.Types
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props

open Client
open Common.Numerals

let translateNumeralRange = function
    | "From0To100" -> Range.From0To100
    | "From100To1000" -> Range.From100To1000
    | "From1000To1000000" -> Range.From1000To1000000
    | "From1000000" -> Range.From1000000
    | x -> sprintf "Unknown numeral range %s" x |> failwith 

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
        let translate = function | NumeralRangeUnset -> None | x -> Some (translateNumeralRange x)
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
                        Markup.option (string Range.From0To100) "0 - 100"
                        Markup.option (string Range.From100To1000) "100 - 1000"
                        Markup.option (string Range.From1000To1000000) "1000 - 1000000"
                        Markup.option (string Range.From1000000) "1000000+"
                    ]
                ]
        ]
