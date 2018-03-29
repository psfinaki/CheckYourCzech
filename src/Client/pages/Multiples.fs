module Client.Multiples

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Style

type Model = {
    Result : string
}

type Msg = 
    | MarkTrue

let init () =
    { Result = "" }

let view model dispatch =
    [ 
      words 60 "Write multiple for the word" 
      form [] [
        div [ClassName ("form-group")] [
            label [] [
                str "Panda"
            ]

            input [ 
                HTMLAttr.Type "text"
            ]

            button [ OnClick (fun _ -> dispatch MarkTrue) ] [
                str "OK"
            ]

            label [] [
                str model.Result
            ]

            button [] [
                str "Repeat"
            ]
        ]
      ]
    ]