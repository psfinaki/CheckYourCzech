module Menu

open Fable.Helpers.React
open Style
open Pages

let view () =
    div [ centerStyle "row" ] [
          yield viewLink Page.Home "Home"
          yield viewLink Page.Plural "Plural"
        ]