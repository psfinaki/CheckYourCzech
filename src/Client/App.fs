module App

open Elmish
open Elmish.React
open Elmish.Browser.UrlParser
open Elmish.Browser.Navigation
open Fable.Helpers.React
open Pages

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

let urlParser location = parseHash pageParser location

type Model =
    | Home
    | Plurals of Plurals.Model
    | Accusatives of Accusatives.Model
    | Comparatives of Comparatives.Model
    | Imperatives of Imperatives.Model
    | Participles of Participles.Model

type Msg = 
    | PluralsMsg of Plurals.Msg
    | AccusativesMsg of Accusatives.Msg
    | ComparativesMsg of Comparatives.Msg
    | ImperativesMsg of Imperatives.Msg
    | ParticiplesMsg of Participles.Msg

let viewPage model dispatch =
    match model with
    | Home ->
        Home.view ()
    | Plurals m ->
        Plurals.view m (PluralsMsg >> dispatch)
    | Accusatives m ->
        Accusatives.view m (AccusativesMsg >> dispatch)
    | Comparatives m ->
        Comparatives.view m (ComparativesMsg >> dispatch)
    | Imperatives m ->
        Imperatives.view m (ImperativesMsg >> dispatch)
    | Participles m ->
        Participles.view m (ParticiplesMsg >> dispatch)

let urlUpdate (result:Page option) model =
    match result with
    | None ->
        ( model, Navigation.modifyUrl (Pages.toHash Page.Home) )
    | Some Page.Home ->
        Home, Cmd.none
    | Some Page.Plurals ->
        let m, cmd = Plurals.init()
        Plurals m, Cmd.map PluralsMsg cmd
    | Some Page.Accusatives ->
        let m, cmd = Accusatives.init()
        Accusatives m, Cmd.map AccusativesMsg cmd
    | Some Page.Comparatives ->
        let m, cmd = Comparatives.init()
        Comparatives m, Cmd.map ComparativesMsg cmd
    | Some Page.Imperatives ->
        let m, cmd = Imperatives.init()
        Imperatives m, Cmd.map ImperativesMsg cmd
    | Some Page.Participles ->
        let m, cmd = Participles.init()
        Participles m, Cmd.map ParticiplesMsg cmd

let init result = urlUpdate result Home

let update msg model =
    match msg, model with
    | PluralsMsg msg, Plurals m ->
        let m, cmd = Plurals.update msg m
        Plurals m, Cmd.map PluralsMsg cmd
    | AccusativesMsg msg, Accusatives m ->
        let m, cmd = Accusatives.update msg m
        Accusatives m, Cmd.map AccusativesMsg cmd
    | ComparativesMsg msg, Comparatives m ->
        let m, cmd = Comparatives.update msg m
        Comparatives m, Cmd.map ComparativesMsg cmd
    | ImperativesMsg msg, Imperatives m ->
        let m, cmd = Imperatives.update msg m
        Imperatives m, Cmd.map ImperativesMsg cmd
    | ParticiplesMsg msg, Participles m ->
        let m, cmd = Participles.update msg m
        Participles m, Cmd.map ParticiplesMsg cmd
    | _, _ ->
        model, Cmd.none

let view model dispatch =
    div [] [ 
        Menu.view()
        hr []
        div [ Styles.center "column" ] (viewPage model dispatch)
    ]

Program.mkProgram init update view
|> Program.toNavigable urlParser urlUpdate
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
