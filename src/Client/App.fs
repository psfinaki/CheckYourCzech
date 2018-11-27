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

type PageModel =
    | HomeModel
    | PluralsModel of Plurals.Model
    | AccusativesModel of Accusatives.Model
    | ComparativesModel of Comparatives.Model
    | ImperativesModel of Imperatives.Model
    | ParticiplesModel of Participles.Model

type Msg = 
    | PluralsMsg of Plurals.Msg
    | AccusativesMsg of Accusatives.Msg
    | ComparativesMsg of Comparatives.Msg
    | ImperativesMsg of Imperatives.Msg
    | ParticiplesMsg of Participles.Msg

let viewPage model dispatch =
    match model with
    | HomeModel ->
        Home.view ()
    | PluralsModel m ->
        Plurals.view m (PluralsMsg >> dispatch)
    | AccusativesModel m ->
        Accusatives.view m (AccusativesMsg >> dispatch)
    | ComparativesModel m ->
        Comparatives.view m (ComparativesMsg >> dispatch)
    | ImperativesModel m ->
        Imperatives.view m (ImperativesMsg >> dispatch)
    | ParticiplesModel m ->
        Participles.view m (ParticiplesMsg >> dispatch)

let urlUpdate (result:Page option) model =
    match result with
    | None ->
        ( model, Navigation.modifyUrl (Pages.toHash Page.Home) )
    | Some Page.Home ->
        HomeModel, Cmd.none
    | Some Page.Plurals ->
        let m, cmd = Plurals.init()
        PluralsModel m, Cmd.map PluralsMsg cmd
    | Some Page.Accusatives ->
        let m, cmd = Accusatives.init()
        AccusativesModel m, Cmd.map AccusativesMsg cmd
    | Some Page.Comparatives ->
        let m, cmd = Comparatives.init()
        ComparativesModel m, Cmd.map ComparativesMsg cmd
    | Some Page.Imperatives ->
        let m, cmd = Imperatives.init()
        ImperativesModel m, Cmd.map ImperativesMsg cmd
    | Some Page.Participles ->
        let m, cmd = Participles.init()
        ParticiplesModel m, Cmd.map ParticiplesMsg cmd

let init result = urlUpdate result HomeModel

let update msg model =
    match msg, model with
    | PluralsMsg msg, PluralsModel m ->
        let m, cmd = Plurals.update msg m
        PluralsModel m, Cmd.map PluralsMsg cmd
    | AccusativesMsg msg, AccusativesModel m ->
        let m, cmd = Accusatives.update msg m
        AccusativesModel m, Cmd.map AccusativesMsg cmd
    | ComparativesMsg msg, ComparativesModel m ->
        let m, cmd = Comparatives.update msg m
        ComparativesModel m, Cmd.map ComparativesMsg cmd
    | ImperativesMsg msg, ImperativesModel m ->
        let m, cmd = Imperatives.update msg m
        ImperativesModel m, Cmd.map ImperativesMsg cmd
    | ParticiplesMsg msg, ParticiplesModel m ->
        let m, cmd = Participles.update msg m
        ParticiplesModel m, Cmd.map ParticiplesMsg cmd
    | _, _ ->
        model, Cmd.none

let view model dispatch =
    div [] [ 
        Menu.view ()
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
