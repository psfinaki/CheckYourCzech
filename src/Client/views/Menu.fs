module Client.Menu

open Fable.Helpers.React
open Client.Style
open Client.Pages
open ServerCode.Domain

type Model = UserData option

let view () =
    div [ centerStyle "row" ] [
          yield viewLink Page.Home "Home"
          yield viewLink Page.WishList "Wishlist"
          yield viewLink Page.Multiples "Multiples"
        ]