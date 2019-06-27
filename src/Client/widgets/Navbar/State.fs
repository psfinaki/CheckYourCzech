module Navbar.State

open Types

let init () =
    { isBurgerOpen = false }, []

let update msg model =
    match msg with
    | ToggleBurger -> { isBurgerOpen = not model.isBurgerOpen }, []
