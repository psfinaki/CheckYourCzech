module Client.App

open Fable.Core

open Fable.Import
open Fable.PowerPack
open Elmish
open Elmish.React
open Fable.Import.Browser
open Elmish.Browser.Navigation
open Elmish.HMR
open Client.Pages
open ServerCode.Domain

JsInterop.importSideEffects "whatwg-fetch"
JsInterop.importSideEffects "babel-polyfill"

/// The composed model for the different possible page states of the application
type PageModel =
    | HomePageModel
    | WishListModel of WishList.Model
    | MultiplesModel

/// The composed model for the application, which is a single page state plus login information
type Model =
    { User : UserData option
      PageModel : PageModel }

/// The composed set of messages that update the state of the application
type Msg =
    | LoggedIn of UserData
    | StorageFailure of exn
    | WishListMsg of WishList.Msg

/// The navigation logic of the application given a page identity parsed from the .../#info 
/// information in the URL.
let urlUpdate (result:Page option) model =
    match result with
    | None ->
        Browser.console.error("Error parsing url: " + Browser.window.location.href)
        ( model, Navigation.modifyUrl (toHash Page.Home) )

    | Some Page.WishList ->
        let m = WishList.init ()
        { model with PageModel = WishListModel m }, Cmd.none

    | Some Page.Home ->
        { model with PageModel = HomePageModel }, Cmd.none

    | Some Page.Multiples ->
        { model with PageModel = MultiplesModel }, []

let loadUser () : UserData option =
    BrowserLocalStorage.load "user"

let saveUserCmd user =
    Cmd.ofFunc (BrowserLocalStorage.save "user") user (fun _ -> LoggedIn user) StorageFailure

let init result =
    let user = loadUser ()
    let model =
        { User = user
          PageModel = HomePageModel }

    urlUpdate result model

let update msg model =
    match msg, model.PageModel with
    | StorageFailure e, _ ->
        printfn "Unable to access local storage: %A" e
        model, Cmd.none

    | WishListMsg msg, WishListModel m ->
        let m, cmd = WishList.update msg m
        { model with
            PageModel = WishListModel m }, Cmd.map WishListMsg cmd

    | WishListMsg _, _ -> 
        model, Cmd.none

    | LoggedIn newUser, _ ->
        let nextPage = Page.WishList
        { model with User = Some newUser },
            Cmd.batch [
                saveUserCmd newUser
                Navigation.newUrl (toHash nextPage) ]

// VIEW

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Client.Style

/// Constructs the view for a page given the model and dispatcher.
let viewPage model dispatch =
    match model.PageModel with
    | HomePageModel ->
        Home.view ()

    | WishListModel m ->
        [ WishList.view m (WishListMsg >> dispatch) ]

    | MultiplesModel ->
        Multiples.view()

/// Constructs the view for the application given the model.
let view model dispatch =
    div [] [ 
        Menu.view ()
        hr []
        div [ centerStyle "column" ] (viewPage model dispatch)
    ]

open Elmish.React
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
