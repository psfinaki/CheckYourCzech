module Client.App

open Fable.Core
open Fable.Core.JsInterop

open Fable.Import
open Elmish
open Elmish.React
open Elmish.Browser.Navigation
open Elmish.HMR
open Client.Shared
open Client.Pages

JsInterop.importSideEffects "whatwg-fetch"
JsInterop.importSideEffects "babel-polyfill"

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
        let m, cmd = Multiples.init()
        { model with PageModel = MultiplesModel m }, Cmd.map MultiplesMsg cmd

let init result =
    let model =
        { PageModel = HomePageModel }

    urlUpdate result model

let update msg model =
    match msg, model.PageModel with
    | MultiplesMsg msg, MultiplesModel m ->
        let m = Multiples.update msg m
        { model with PageModel = MultiplesModel m }, Cmd.none
    | MultiplesMsg _, _ ->
        model, Cmd.none

open Elmish.Debug

let withReact =
    if (!!Browser.window?__INIT_MODEL__)
    then Program.withReactHydrate
    else Program.withReact


// App
Program.mkProgram init update view
|> Program.toNavigable Pages.urlParser urlUpdate
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
