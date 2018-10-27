module Shared

/// The composed model for the different possible page states of the application
type PageModel =
    | HomePageModel
    | PluralsModel of Plurals.Model
    | ComparativesModel of Comparatives.Model
    | ImperativesModel of Imperatives.Model
    | ParticiplesModel of Participles.Model

/// The composed model for the application, which is a single page state plus login information
type Model =
    { PageModel : PageModel }

/// The composed set of messages that update the state of the application
type Msg = 
    | PluralsMsg of Plurals.Msg
    | ComparativesMsg of Comparatives.Msg
    | ImperativesMsg of Imperatives.Msg
    | ParticiplesMsg of Participles.Msg

// VIEW

open Fable.Helpers.React

/// Constructs the view for a page given the model and dispatcher.
let viewPage model dispatch =
    match model.PageModel with
    | HomePageModel ->
        Home.view ()
    | PluralsModel m ->
        Plurals.view m (PluralsMsg >> dispatch)
    | ComparativesModel m ->
        Comparatives.view m (ComparativesMsg >> dispatch)
    | ImperativesModel m ->
        Imperatives.view m (ImperativesMsg >> dispatch)
    | ParticiplesModel m ->
        Participles.view m (ParticiplesMsg >> dispatch)

/// Constructs the view for the application given the model.
let view model dispatch =
    div [] [ 
        Menu.view ()
        hr []
        div [ Styles.center "column" ] (viewPage model dispatch)
    ]