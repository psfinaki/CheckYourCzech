module Rule

open Fable.Helpers.React

let view rule =
    div []
        [
            Markup.toggleLink "rules" rule
        ]

