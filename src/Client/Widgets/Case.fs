module Client.Widgets.Case

open Browser.Types
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props

open Client
open Common.GrammarCategories.Common
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

    div [ClassName "filter case-filter"] 
        [
            div [ ClassName "case-filter-label" ] 
                [
                    label [] [ str "Case" ]
                ]

            div [ ] 
                [
                    Markup.select selectedValue handleChangeCase [
                        Markup.option CaseUnset "Any"
                        Markup.option $"{Case.Nominative}" "Nominative"
                        Markup.option $"{Case.Genitive}" "Genitive"
                        Markup.option $"{Case.Dative}" "Dative"
                        Markup.option $"{Case.Accusative}" "Accusative"
                        Markup.option $"{Case.Vocative}" "Vocative"
                        Markup.option $"{Case.Locative}" "Locative"
                        Markup.option $"{Case.Instrumental}" "Instrumental"
                    ]
                ]
        ]
