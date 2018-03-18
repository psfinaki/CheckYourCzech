module Client.Multiples

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Style

let view() =
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

            button [] [
                str "OK"
            ]

            label [] [
                str "Correct!"
            ]

            button [] [
                str "Repeat"
            ]
        ]
      ]
    ]