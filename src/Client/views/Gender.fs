module Gender

open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Import.React
open Gender

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

let view dispatch =
    let handleChangeGender (event: FormEvent) =
        let translate = function | GenderUnset -> None | x -> Some (Gender.FromString x)
        dispatch (SetGender (translate !!event.target?value))

    div [ Styles.row ] 
        [
            div [ Style [ Height "50%" ] ] 
                [
                    Markup.label Styles.whiteLabel (str "Gender") 
                ]

            div [ Styles.select ] 
                [
                    Markup.select handleChangeGender [
                        Markup.option GenderUnset "Any"
                        Markup.option (Gender.ToString MasculineAnimate) "Masculine Animate"
                        Markup.option (Gender.ToString MasculineInanimate) "Masculine Inanimate"
                        Markup.option (Gender.ToString Feminine) "Feminine"
                        Markup.option (Gender.ToString Neuter) "Neuter"
                    ]
                ]
        ]
