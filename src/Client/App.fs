module Client.App

open Fable.Core

open Fable.Import
open Elmish
open Elmish.React
open Elmish.Browser.Navigation
open Elmish.HMR
open Client.Pages

JsInterop.importSideEffects "whatwg-fetch"
JsInterop.importSideEffects "babel-polyfill"

type Msg = 
    | MultiplesMsg of Multiples.Msg

/// The composed model for the different possible page states of the application
type PageModel =
    | HomePageModel
    | MultiplesModel of Multiples.Model

/// The composed model for the application, which is a single page state plus login information
type Model =
    { PageModel : PageModel }

/// The navigation logic of the application given a page identity parsed from the .../#info 
/// information in the URL.
let urlUpdate (result:Page option) model =
    match result with
    | None ->
        Browser.console.error("Error parsing url: " + Browser.window.location.href)
        ( model, Navigation.modifyUrl (toHash Page.Home) )

    | Some Page.Home ->
        { model with PageModel = HomePageModel }, Cmd.none

    | Some Page.Multiples ->
        let m = Multiples.init()
        { model with PageModel = MultiplesModel m }, Cmd.none

let init result =
    let model =
        { PageModel = HomePageModel }

    urlUpdate result model

let update msg model =
    match msg, model.PageModel with
    | MultiplesMsg _, MultiplesModel m ->
        let m = { m with Result = "Success!" }
        { model with PageModel = MultiplesModel m }, Cmd.none
    | MultiplesMsg _, _ ->
        model, Cmd.none

// VIEW

open Fable.Helpers.React
open Client.Style

/// Constructs the view for a page given the model and dispatcher.
let viewPage model dispatch =
    match model.PageModel with
    | HomePageModel ->
        Home.view ()

    | MultiplesModel m ->
        Multiples.view m (MultiplesMsg >> dispatch)

/// Constructs the view for the application given the model.
let view model dispatch =
    div [] [ 
        Menu.view ()
        hr []
        div [ centerStyle "column" ] (viewPage model dispatch)
    ]

open Elmish.Debug

// App
Program.mkProgram init update view
|> Program.toNavigable Pages.urlParser urlUpdate
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
