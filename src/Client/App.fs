module Client.App

open Elmish
open Elmish.React
open Elmish.UrlParser
open Elmish.Navigation
open Fulma
open Fable.React

open Client
open Client.AppPages
open Client.Styles
open Client.Widgets
open Client.Pages

#if DEBUG
open Elmish.Debug
#endif

// must be the last one in Elmish:
// https://elmish.github.io/hmr/#Usage
open Elmish.HMR

let urlParser location = parseHash pageParser location

type PageModel =
    | Home
    | NounDeclension of NounDeclension.Model
    | AdjectiveDeclension of AdjectiveDeclension.Model
    | AdjectiveComparatives of AdjectiveComparatives.Model
    | VerbImperatives of VerbImperatives.Model
    | VerbParticiples of VerbParticiples.Model
    | VerbConjugation of VerbConjugation.Model

type Model = {
    CurrentPage: PageModel
}

type Msg = 
    | NounDeclensionMsg of NounDeclension.Msg
    | AdjectiveDeclensionMsg of AdjectiveDeclension.Msg
    | AdjectiveComparativesMsg of AdjectiveComparatives.Msg
    | VerbImperativesMsg of VerbImperatives.Msg
    | VerbParticiplesMsg of VerbParticiples.Msg
    | VerbConjugationMsg of VerbConjugation.Msg

let viewPage model dispatch =
    match model.CurrentPage with
    | Home ->
        Home.view ()
    | NounDeclension m ->
        NounDeclension.view m (NounDeclensionMsg >> dispatch)
    | AdjectiveDeclension m ->
        AdjectiveDeclension.view m (AdjectiveDeclensionMsg >> dispatch)
    | AdjectiveComparatives m ->
        AdjectiveComparatives.view m (AdjectiveComparativesMsg >> dispatch)
    | VerbImperatives m ->
        VerbImperatives.view m (VerbImperativesMsg >> dispatch)
    | VerbParticiples m ->
        VerbParticiples.view m (VerbParticiplesMsg >> dispatch)
    | VerbConjugation m ->
        VerbConjugation.view m (VerbConjugationMsg >> dispatch)

let updateModelPage model newPage = 
    {model with CurrentPage = newPage}

let urlUpdate (result:Page option) model =
    match result with
    | None ->
        ( model, Navigation.modifyUrl (toHash Page.Home) )
    | Some Page.Home ->
        updateModelPage model Home, Cmd.none
    | Some Page.NounDeclension ->
        let m, cmd = NounDeclension.init()
        updateModelPage model (NounDeclension m), Cmd.map NounDeclensionMsg cmd
    | Some Page.AdjectiveDeclension ->
        let m, cmd = AdjectiveDeclension.init()
        updateModelPage model (AdjectiveDeclension m), Cmd.map AdjectiveDeclensionMsg cmd
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
    urlUpdate result {CurrentPage = Home}

let update msg model =
    match msg, model.CurrentPage with
    | NounDeclensionMsg msg, NounDeclension m ->
        let m, cmd = NounDeclension.update msg m
        updateModelPage model (NounDeclension m), Cmd.map NounDeclensionMsg cmd
    | AdjectiveDeclensionMsg msg, AdjectiveDeclension m ->
        let m, cmd = AdjectiveDeclension.update msg m
        updateModelPage model (AdjectiveDeclension m), Cmd.map AdjectiveDeclensionMsg cmd
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
    | _, _ ->
        model, Cmd.none

let view model dispatch =
    let isHome = 
        match model.CurrentPage with 
        | Home -> true
        | _ -> false
    div [] [ 
        Navbar.root isHome dispatch
        div [ center "column" ] (viewPage model dispatch)
    ]

Program.mkProgram init update view
|> Program.toNavigable urlParser urlUpdate
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
