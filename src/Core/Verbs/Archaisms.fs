module Archaisms

open StringHelper

let hasArchaicEnding verb = 
    verb |> ends "ti" || 
    verb |> ends "ci"

let isArchaic = Reflexives.removeReflexive >> hasArchaicEnding

let isModern = not << isArchaic
