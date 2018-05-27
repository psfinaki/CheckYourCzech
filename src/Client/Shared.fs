module Client.Shared

/// The composed model for the different possible page states of the application
type PageModel =
    | HomePageModel
    | PluralModel of Plural.Model

/// The composed model for the application, which is a single page state plus login information
type Model =
    { PageModel : PageModel }

/// The composed set of messages that update the state of the application
type Msg = 
    | PluralMsg of Plural.Msg

// VIEW

open Fable.Helpers.React
open Client.Style

/// Constructs the view for a page given the model and dispatcher.
let viewPage model dispatch =
    match model.PageModel with
    | HomePageModel ->
        Home.view ()

    | PluralModel m ->
        Plural.view m (PluralMsg >> dispatch)

/// Constructs the view for the application given the model.
let view model dispatch =
    div [] [ 
        Menu.view ()
        hr []
        div [ centerStyle "column" ] (viewPage model dispatch)
    ]