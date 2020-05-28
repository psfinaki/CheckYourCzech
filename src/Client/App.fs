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
    | NounDeclension of NounDeclension.Model
    | NounPlurals of NounPlurals.Model
    | NounAccusatives of NounAccusatives.Model
    | AdjectivePlurals of AdjectivePlurals.Model
    | AdjectiveComparatives of AdjectiveComparatives.Model
    | VerbImperatives of VerbImperatives.Model
    | VerbParticiples of VerbParticiples.Model
    | VerbConjugation of VerbConjugation.Model

type Model = {
    CurrentPage: PageModel
    Navbar: Navbar.Types.Model
}

type Msg = 
    | NounDeclensionMsg of NounDeclension.Msg
    | NounPluralsMsg of NounPlurals.Msg
    | NounAccusativesMsg of NounAccusatives.Msg
    | AdjectivePluralsMsg of AdjectivePlurals.Msg
    | AdjectiveComparativesMsg of AdjectiveComparatives.Msg
    | VerbImperativesMsg of VerbImperatives.Msg
    | VerbParticiplesMsg of VerbParticiples.Msg
    | VerbConjugationMsg of VerbConjugation.Msg
    | NavbarMsg of Navbar.Types.Msg

let viewPage model dispatch =
    match model.CurrentPage with
    | Home ->
        Home.view ()
    | NounDeclension m ->
        NounDeclension.view m (NounDeclensionMsg >> dispatch)
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
    | VerbConjugation m ->
        VerbConjugation.view m (VerbConjugationMsg >> dispatch)

let updateModelPage model newPage = 
    let resetNavbar = {model.Navbar with isBurgerOpen = false}
    {model with CurrentPage = newPage; Navbar = resetNavbar}

let updateModelNavbar model newNavbar = 
    {model with Navbar = newNavbar}

let urlUpdate (result:Page option) model =
    match result with
    | None ->
        ( model, Navigation.modifyUrl (Pages.toHash Page.Home) )
    | Some Page.Home ->
        updateModelPage model Home, Cmd.none
    | Some Page.NounDeclension ->
        let m, cmd = NounDeclension.init()
        updateModelPage model (NounDeclension m), Cmd.map NounDeclensionMsg cmd
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
    | Some Page.VerbConjugation ->
        let m, cmd = VerbConjugation.init()
        updateModelPage model (VerbConjugation m), Cmd.map VerbConjugationMsg cmd    

let init result = 
    Logger.setup()
    let m = Navbar.State.init()
    urlUpdate result {CurrentPage = Home; Navbar = m}

let update msg model =
    match msg, model.CurrentPage with
    | NounPluralsMsg msg, NounPlurals m ->
        let m, cmd = NounPlurals.update msg m
        updateModelPage model (NounPlurals m), Cmd.map NounPluralsMsg cmd
    | NounDeclensionMsg msg, NounDeclension m ->
        let m, cmd = NounDeclension.update msg m
        updateModelPage model (NounDeclension m), Cmd.map NounDeclensionMsg cmd
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
    | VerbConjugationMsg msg, VerbConjugation m ->
        let m, cmd = VerbConjugation.update msg m
        updateModelPage model (VerbConjugation m), Cmd.map VerbConjugationMsg cmd    
    | NavbarMsg msg, _ ->
        let m, cmd = Navbar.State.update msg model.Navbar
        updateModelNavbar model m, Cmd.map NavbarMsg cmd
    | _, _ ->
        model, Cmd.none



let view model dispatch =
    div [] [ 
        Navbar.View.root model.Navbar (NavbarMsg >> dispatch)
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
