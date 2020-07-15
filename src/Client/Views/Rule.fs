module Client.Views.Rule

open Fable.React

open Client

let view rule =
    div []
        [
            Markup.toggleLink "rules" rule
        ]
