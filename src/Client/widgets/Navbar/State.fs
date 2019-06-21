module Navbar.State

open Types

let init () =
    { isBurgerOpen = false }, []

let update msg model getTask =
    match msg with
    | ToggleBurger -> { isBurgerOpen = not model.isBurgerOpen }, []
