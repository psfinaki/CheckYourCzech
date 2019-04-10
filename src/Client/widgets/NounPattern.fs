module NounPattern

open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Import.React
open Genders

[<Literal>]
let PatternUnset = ""

let genderPatterns =
    dict [ (MasculineAnimate, ["pan"; "muž"; "předseda"; "soudce"])
           (MasculineInanimate, ["hrad"; "stroj"])
           (Feminine, ["žena"; "růže"; "píseň"; "kost"])
           (Neuter, ["město"; "kuře"; "moře"; "stavení"]) ]

let getGenderPatterns gender = genderPatterns.[gender]

type Model = { 
    Gender : Gender option
    Pattern : string option
}

type Msg =
    | SetGender of Gender option
    | SetPattern of string option

let init() =
    { Gender = None
      Pattern = None }

let update msg model =
    match msg with
    | SetGender gender ->
        { model with Gender = gender; Pattern = None }
    | SetPattern pattern ->
        { model with Pattern = pattern }

let view model dispatch =
    let handleChangePattern (event: FormEvent) =
        let translate = function | PatternUnset -> None | x -> Some x
        dispatch (SetPattern (translate !!event.target?value))
        
    let options = 
        model.Gender
        |> Option.map getGenderPatterns
        |> Option.defaultValue []
        |> Seq.map Markup.simpleOption
        |> Seq.append [Markup.option PatternUnset "Any" ]

    let selectedValue = model.Pattern |> Option.defaultValue "Any"

    div [] 
        [
            div [ Style [ Height "50%" ] ] 
                [
                    Markup.label Styles.whiteLabel (str "Pattern") 
                ]

            div [ Styles.select ] 
                [
                    Markup.select selectedValue handleChangePattern options
                ]
        ]
