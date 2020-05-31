module Client.Views.Rule

open Fable.Helpers.React

open Client

let view rule =
    div []
        [
            Markup.toggleLink "rules" rule
        ]
