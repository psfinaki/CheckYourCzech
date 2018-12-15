module Verb

open StringHelper

let removeReflexive = remove " se" >> remove " si"

let hasArchaicEnding verb = 
    verb |> ends "ti" || 
    verb |> ends "ci"

let isArchaic = removeReflexive >> hasArchaicEnding
