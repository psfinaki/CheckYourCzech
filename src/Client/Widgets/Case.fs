﻿module Client.Widgets.Case

open Browser.Types
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props

open Client
open Common.GrammarCategories.Nouns
open Common.Utils

[<Literal>]
let CaseUnset = ""

type Model = {
    Case : Case option
}

type Msg =
    | SetCase of Case option

let init() =
    { Case = None }

let update msg model =
    match msg with
    | SetCase case ->
        { model with Case = case }

let view model dispatch =
    let handleChangeCase (event: Event) =
        let translate = function | CaseUnset -> None | x -> Some (parseUnionCase<Case> x)
        dispatch (SetCase (translate !!event.target?value))

    let selectedValue = model.Case |> Option.map string |> Option.defaultValue "Any"

    div [ClassName "case-filter"] 
        [
            div [ ClassName "case-filter-label" ] 
                [
                    label [] [ str "Case" ]
                ]

            div [ ] 
                [
                    Markup.select selectedValue handleChangeCase [
                        Markup.option CaseUnset "Any"
                        Markup.option (string Case.Nominative) "Nominative"
                        Markup.option (string Case.Genitive) "Genitive"
                        Markup.option (string Case.Dative) "Dative"
                        Markup.option (string Case.Accusative) "Accusative"
                        Markup.option (string Case.Vocative) "Vocative"
                        Markup.option (string Case.Locative) "Locative"
                        Markup.option (string Case.Instrumental) "Instrumental"
                    ]
                ]
        ]
