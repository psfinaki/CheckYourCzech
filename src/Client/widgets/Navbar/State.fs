module Navbar.State

open Elmish
open Types

let init () =
    { isBurgerOpen = false }

let update msg model =
    match msg with
    | ToggleBurger -> { isBurgerOpen = not model.isBurgerOpen }, Cmd.none
