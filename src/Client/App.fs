module App

open Elmish
open Elmish.React
open Elmish.Browser.UrlParser
open Elmish.Browser.Navigation
open Pages
open Fulma
open Fable.FontAwesome
open Fable.FontAwesome.Free
open Fable.Helpers.React
open Fable.Helpers.React.Props

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

let urlParser location = parseHash pageParser location

type PageModel =
    | Home
    | NounPlurals of NounPlurals.Model
    | NounAccusatives of NounAccusatives.Model
    | AdjectivePlurals of AdjectivePlurals.Model
    | AdjectiveComparatives of AdjectiveComparatives.Model
    | VerbImperatives of VerbImperatives.Model
    | VerbParticiples of VerbParticiples.Model

type Model = {
    currentPage: PageModel
    navbar: Navbar.Types.Model
}

type Msg = 
    | NounPluralsMsg of NounPlurals.Msg
    | NounAccusativesMsg of NounAccusatives.Msg
    | AdjectivePluralsMsg of AdjectivePlurals.Msg
    | AdjectiveComparativesMsg of AdjectiveComparatives.Msg
    | VerbImperativesMsg of VerbImperatives.Msg
    | VerbParticiplesMsg of VerbParticiples.Msg
    | NavbarMsg of Navbar.Types.Msg

let viewPage model dispatch =
    match model.currentPage with
    | Home ->
        Home.view ()
    | NounPlurals m ->
        NounPlurals.view m (NounPluralsMsg >> dispatch)
    | NounAccusatives m ->
        NounAccusatives.view m (NounAccusativesMsg >> dispatch)
    | AdjectivePlurals m ->
        AdjectivePlurals.view m (AdjectivePluralsMsg >> dispatch)
    | AdjectiveComparatives m ->
        AdjectiveComparatives.view m (AdjectiveComparativesMsg >> dispatch)
    | VerbImperatives m ->
        VerbImperatives.view m (VerbImperativesMsg >> dispatch)
    | VerbParticiples m ->
        VerbParticiples.view m (VerbParticiplesMsg >> dispatch)

// TODO refactor this part using converter functions
let updateModelPage model newPage = 
    {model with currentPage = newPage}

let updateModelNavbar model newNavbar = 
    {model with navbar = newNavbar}

let urlUpdate (result:Page option) model =
    match result with
    | None ->
        ( model, Navigation.modifyUrl (Pages.toHash Page.Home) )
    | Some Page.Home ->
        updateModelPage model Home, Cmd.none
    | Some Page.NounPlurals ->
        let m, cmd = NounPlurals.init()
        updateModelPage model (NounPlurals m), Cmd.map NounPluralsMsg cmd
    | Some Page.NounAccusatives ->
        let m, cmd = NounAccusatives.init()
        updateModelPage model (NounAccusatives m), Cmd.map NounAccusativesMsg cmd
    | Some Page.AdjectivePlurals ->
        let m, cmd = AdjectivePlurals.init()
        updateModelPage model (AdjectivePlurals m), Cmd.map AdjectivePluralsMsg cmd
    | Some Page.AdjectiveComparatives ->
        let m, cmd = AdjectiveComparatives.init()
        updateModelPage model (AdjectiveComparatives m), Cmd.map AdjectiveComparativesMsg cmd
    | Some Page.VerbImperatives ->
        let m, cmd = VerbImperatives.init()
        updateModelPage model (VerbImperatives m), Cmd.map VerbImperativesMsg cmd
    | Some Page.VerbParticiples ->
        let m, cmd = VerbParticiples.init()
        updateModelPage model (VerbParticiples m), Cmd.map VerbParticiplesMsg cmd

let init result = 
    Logger.setup()
    let m, _ = Navbar.State.init()
    urlUpdate result {currentPage = Home; navbar = m}

let update msg model =
    match msg, model.currentPage with
    | NounPluralsMsg msg, NounPlurals m ->
        let m, cmd = NounPlurals.update msg m
        updateModelPage model (NounPlurals m), Cmd.map NounPluralsMsg cmd
    | NounAccusativesMsg msg, NounAccusatives m ->
        let m, cmd = NounAccusatives.update msg m
        updateModelPage model (NounAccusatives m), Cmd.map NounAccusativesMsg cmd
    | AdjectivePluralsMsg msg, AdjectivePlurals m ->
        let m, cmd = AdjectivePlurals.update msg m
        updateModelPage model (AdjectivePlurals m), Cmd.map AdjectivePluralsMsg cmd
    | AdjectiveComparativesMsg msg, AdjectiveComparatives m ->
        let m, cmd = AdjectiveComparatives.update msg m
        updateModelPage model (AdjectiveComparatives m), Cmd.map AdjectiveComparativesMsg cmd
    | VerbImperativesMsg msg, VerbImperatives m ->
        let m, cmd = VerbImperatives.update msg m
        updateModelPage model (VerbImperatives m), Cmd.map VerbImperativesMsg cmd
    | VerbParticiplesMsg msg, VerbParticiples m ->
        let m, cmd = VerbParticiples.update msg m
        updateModelPage model (VerbParticiples m), Cmd.map VerbParticiplesMsg cmd
    | NavbarMsg msg, _ ->
        let m, cmd = Navbar.State.update msg model.navbar
        updateModelNavbar model m, Cmd.map NavbarMsg cmd
    | _, _ ->
        model, Cmd.none



let view model dispatch =
    div [] [ 
        Navbar.View.root model.navbar (NavbarMsg >> dispatch)
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
