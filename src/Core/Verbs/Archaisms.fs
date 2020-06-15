module Core.Verbs.Archaisms

open Common.StringHelper
open Core.Reflexives

let hasArchaicEnding verb = 
    verb |> ends "ti" || 
    verb |> ends "ci"

let isArchaic = removeReflexive >> hasArchaicEnding

let isModern = not << isArchaic
