module Reflexives

open StringHelper

let removeReflexive = remove " se" >> remove " si"

let isReflexive word = 
    word |> ends " se" ||
    word |> ends " si"

let getReflexive = function
    | word when word |> ends " se" -> "se"
    | word when word |> ends " si" -> "si"
    | _ -> invalidArg "verb" "Incorrect reflexive verb"

