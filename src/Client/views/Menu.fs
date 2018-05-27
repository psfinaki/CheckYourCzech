module Client.Menu

open Fable.Helpers.React
open Client.Style
open Client.Pages

let view () =
    div [ centerStyle "row" ] [
          yield viewLink Page.Home "Home"
          yield viewLink Page.Plural "Plural"
        ]