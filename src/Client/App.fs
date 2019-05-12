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
    | Plurals of NounPlurals.Model
    | Accusatives of NounAccusatives.Model
    | Comparatives of AdjectiveComparatives.Model
    | Imperatives of VerbImperatives.Model
    | Participles of VerbParticiples.Model

type Msg = 
    | PluralsMsg of NounPlurals.Msg
    | AccusativesMsg of NounAccusatives.Msg
    | ComparativesMsg of AdjectiveComparatives.Msg
    | ImperativesMsg of VerbImperatives.Msg
    | ParticiplesMsg of VerbParticiples.Msg

let viewPage model dispatch =
    match model with
    | Home ->
        Home.view ()
    | Plurals m ->
        NounPlurals.view m (PluralsMsg >> dispatch)
    | Accusatives m ->
        NounAccusatives.view m (AccusativesMsg >> dispatch)
    | Comparatives m ->
        AdjectiveComparatives.view m (ComparativesMsg >> dispatch)
    | Imperatives m ->
        VerbImperatives.view m (ImperativesMsg >> dispatch)
    | Participles m ->
        VerbParticiples.view m (ParticiplesMsg >> dispatch)

let urlUpdate (result:Page option) model =
    match result with
    | None ->
        ( model, Navigation.modifyUrl (Pages.toHash Page.Home) )
    | Some Page.Home ->
        Home, Cmd.none
    | Some Page.NounPlurals ->
        let m, cmd = NounPlurals.init()
        Plurals m, Cmd.map PluralsMsg cmd
    | Some Page.NounAccusatives ->
        let m, cmd = NounAccusatives.init()
        Accusatives m, Cmd.map AccusativesMsg cmd
    | Some Page.AdjectiveComparatives ->
        let m, cmd = AdjectiveComparatives.init()
        Comparatives m, Cmd.map ComparativesMsg cmd
    | Some Page.VerbImperatives ->
        let m, cmd = VerbImperatives.init()
        Imperatives m, Cmd.map ImperativesMsg cmd
    | Some Page.VerbParticiples ->
        let m, cmd = VerbParticiples.init()
        Participles m, Cmd.map ParticiplesMsg cmd

let init result = 
    Logger.setup()
    urlUpdate result Home

let update msg model =
    match msg, model with
    | PluralsMsg msg, Plurals m ->
        let m, cmd = NounPlurals.update msg m
        Plurals m, Cmd.map PluralsMsg cmd
    | AccusativesMsg msg, Accusatives m ->
        let m, cmd = NounAccusatives.update msg m
        Accusatives m, Cmd.map AccusativesMsg cmd
    | ComparativesMsg msg, Comparatives m ->
        let m, cmd = AdjectiveComparatives.update msg m
        Comparatives m, Cmd.map ComparativesMsg cmd
    | ImperativesMsg msg, Imperatives m ->
        let m, cmd = VerbImperatives.update msg m
        Imperatives m, Cmd.map ImperativesMsg cmd
    | ParticiplesMsg msg, Participles m ->
        let m, cmd = VerbParticiples.update msg m
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
