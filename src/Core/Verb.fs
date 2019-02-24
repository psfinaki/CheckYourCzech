module Verb

open StringHelper

let removeReflexive = remove " se" >> remove " si"

let hasArchaicEnding verb = 
    verb |> ends "ti" || 
    verb |> ends "ci"

let isArchaic = removeReflexive >> hasArchaicEnding

let getClassByThirdPersonSingular = function
    | form when form |> ends "á"  -> 5
    | form when form |> ends "í"  -> 4
    | form when form |> ends "je" -> 3
    | form when form |> ends "ne" -> 2
    | form when form |> ends "e"  -> 1
    | _ -> invalidArg "verb" "Incorrect third person singular."
