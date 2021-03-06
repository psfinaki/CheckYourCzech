﻿module Client.Widgets.Regularity

open Browser.Types
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props

open Client
open Client.BoolExtensioins

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

let view model dispatch =
    let handleChangeRegularity (event: Event) =
        let translate = function | RegularityUnset -> None | x -> Some (bool.FromString x)
        dispatch (SetRegularity (translate !!event.target?value))

    let selectedValue = model.Regularity |> Option.map bool.AsString |> Option.defaultValue "Any"

    div [ ClassName "filter regularity-filter" ]
        [
            div [ ClassName "regularity-filter-label" ] 
                [
                    label [] [ str "Regularity" ] 
                ]

            div [ ] 
                [
                    Markup.select selectedValue handleChangeRegularity [
                        Markup.option RegularityUnset "Any"
                        Markup.option (bool.AsString true) "Regular"
                        Markup.option (bool.AsString false) "Exceptions"
                    ]
                ]
        ]
