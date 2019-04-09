module Gender

open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Import.React
open Genders

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
    let handleChangeGender (event: FormEvent) =
        let translate = function | GenderUnset -> None | x -> Some (Genders.fromString x)
        dispatch (SetGender (translate !!event.target?value))

    let selectedValue = model.Gender |> Option.map string |> Option.defaultValue "Any"

    div [] 
        [
            div [ Style [ Height "50%" ] ] 
                [
                    Markup.label Styles.whiteLabel (str "Gender") 
                ]

            div [ Styles.select ] 
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
