module App

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Elmish
open Elmish.Debug
open Elmish.React
open Elmish.Browser.Navigation
open Elmish.HMR
open Shared
open Pages

JsInterop.importSideEffects "whatwg-fetch"
JsInterop.importSideEffects "babel-polyfill"

let urlUpdate (result:Page option) model =
    match result with
    | None ->
        Browser.console.error("Error parsing url: " + Browser.window.location.href)
        ( model, Navigation.modifyUrl (toHash Page.Home) )
    | Some Page.Home ->
        { model with PageModel = HomePageModel }, Cmd.none
    | Some Page.Plural ->
        let m, cmd = Plural.init()
        { model with PageModel = PluralModel m }, Cmd.map PluralMsg cmd
    | Some Page.Comparatives ->
        let m, cmd = Comparatives.init()
        { model with PageModel = ComparativesModel m }, Cmd.map ComparativesMsg cmd

let init result =
    let model =
        { PageModel = HomePageModel }

    urlUpdate result model

let update msg model =
    match msg, model.PageModel with
    | PluralMsg msg, PluralModel m ->
        let m, cmd = Plural.update msg m
        { model with PageModel = PluralModel m }, Cmd.map PluralMsg cmd
    | ComparativesMsg msg, ComparativesModel m ->
        let m, cmd = Comparatives.update msg m
        { model with PageModel = ComparativesModel m }, Cmd.map ComparativesMsg cmd
    | _, _ ->
        model, Cmd.none

let withReact =
    if (!!Browser.window?__INIT_MODEL__)
    then Program.withReactHydrate
    else Program.withReact

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
