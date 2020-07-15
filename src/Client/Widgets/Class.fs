module Client.Widgets.Class

open Browser.Types
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props

open Common.Conjugation
open Client

let private parseVerbClass = function
    | "E" -> E
    | "NE" -> NE
    | "JE" -> JE
    | "Í" -> Í
    | "Á" -> Á
    | s -> invalidOp ("odd class: " + s)

[<Literal>]
let ClassUnset = ""

type Model = { 
    Class : VerbClass option
}

type Msg =
    | SetClass of VerbClass option

let init() =
    { Class = None }

let update msg model =
    match msg with
    | SetClass ``class`` ->
        { model with Class = ``class`` }

let view model dispatch =
    let handleChangeClass (event: Event) =
        let translate = function | ClassUnset -> None | x -> Some (parseVerbClass x)
        dispatch (SetClass (translate !!event.target?value))

    let selectedValue = model.Class |> Option.map string |> Option.defaultValue "Any"

    div [ ClassName "class-filter" ] 
        [
            div [ ClassName "class-filter-label" ] 
                [
                    label [] [ str "Class" ]
                ]

            div [ ] 
                [
                    Markup.select selectedValue handleChangeClass [
                        Markup.option ClassUnset "Any"
                        Markup.option (string E) "E"
                        Markup.option (string NE) "NE"
                        Markup.option (string JE) "JE"
                        Markup.option (string Í) "Í"
                        Markup.option (string Á) "Á"
                    ]
                ]
        ]
