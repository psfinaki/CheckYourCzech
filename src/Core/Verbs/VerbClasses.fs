module VerbClasses

open StringHelper
open Verbs

let getClassByThirdPersonSingular = function
    | form when form |> ends "á"  -> Á
    | form when form |> ends "í"  -> Í
    | form when form |> ends "je" -> JE
    | form when form |> ends "ne" -> NE
    | form when form |> ends "e"  -> E
    | _ -> invalidArg "verb" "Incorrect third person singular."
