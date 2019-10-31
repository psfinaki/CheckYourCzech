module VerbClasses

open StringHelper
open VerbArticle
open Reflexives

let getClassByThirdPersonSingular = function
    | form when form |> ends "á"  -> 5
    | form when form |> ends "í"  -> 4
    | form when form |> ends "je" -> 3
    | form when form |> ends "ne" -> 2
    | form when form |> ends "e"  -> 1
    | _ -> invalidArg "verb" "Incorrect third person singular."

let getClass =
    getThirdPersonSingular
    >> Seq.tryExactlyOne
    >> Option.map removeReflexive
    >> Option.map getClassByThirdPersonSingular
