module Rule

open Fable.Helpers.React

let view rule =
    div [ Styles.row ]
        [
            Markup.toggleLink "rules" rule
        ]

