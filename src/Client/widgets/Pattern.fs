module Pattern

open Fable.Core.JsInterop
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.Import.React

[<Literal>]
let PatternUnset = ""

let classPatterns =
    dict [ (1, ["nést"; "číst"; "péct"; "třít"; "brát"; "mazat"])
           (2, ["tisknout"; "minout"; "začít"])
           (3, ["krýt"; "kupovat"])
           (4, ["prosit"; "čistit"; "trpět"; "sázet"])
           (5, ["dělat"]) ]

let getClassPatterns ``class`` = classPatterns.[``class``]

type Model = { 
    Class : int option
    Pattern : string option
}

type Msg =
    | SetClass of int option
    | SetPattern of string option

let init() =
    { Class = None
      Pattern = None }

let update msg model =
    match msg with
    | SetClass ``class`` ->
        { model with Class = ``class`` }
    | SetPattern pattern ->
        { model with Pattern = pattern }

let view model dispatch =
    let handleChangePattern (event: FormEvent) =
        let translate = function | PatternUnset -> None | x -> Some x
        dispatch (SetPattern (translate !!event.target?value))
        
    let options = 
        model.Class
        |> Option.map getClassPatterns
        |> Option.defaultValue []
        |> Seq.map Markup.simpleOption
        |> Seq.append [Markup.option PatternUnset "Any" ]

    div [] 
        [
            div [ Style [ Height "50%" ] ] 
                [
                    Markup.label Styles.whiteLabel (str "Pattern") 
                ]

            div [ Styles.select ] 
                [
                    Markup.select handleChangePattern options
                ]
        ]
