module Client.Widgets.Gender

open Browser.Types
open Fable.Core.JsInterop
open Fable.React
open Fable.React.Props

open Common.GrammarCategories
open Client

let translateGender = function
    | "MasculineAnimate" -> MasculineAnimate
    | "MasculineInanimate" -> MasculineInanimate
    | "Feminine" -> Feminine
    | "Neuter" -> Neuter
    | _ -> failwith "Unknown gender"

[<Literal>]
let GenderUnset = ""

type Model = {
    Gender : Gender option
}

type Msg =
    | SetGender of Gender option

let init() =
    { Gender = None }

let update msg model =
    match msg with
    | SetGender gender ->
        { model with Gender = gender }

let view model dispatch =
    let handleChangeGender (event: Event) =
        let translate = function | GenderUnset -> None | x -> Some (translateGender x)
        dispatch (SetGender (translate !!event.target?value))

    let selectedValue = model.Gender |> Option.map string |> Option.defaultValue "Any"

    div [ClassName "gender-filter"] 
        [
            div [ ClassName "gender-filter-label" ] 
                [
                    label [] [ str "Gender" ]
                ]

            div [ ] 
                [
                    Markup.select selectedValue handleChangeGender [
                        Markup.option GenderUnset "Any"
                        Markup.option (string MasculineAnimate) "Masculine Animate"
                        Markup.option (string MasculineInanimate) "Masculine Inanimate"
                        Markup.option (string Feminine) "Feminine"
                        Markup.option (string Neuter) "Neuter"
                    ]
                ]
        ]
