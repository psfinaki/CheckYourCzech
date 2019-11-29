module VerbValidation

open Article
open Archaisms

let hasRequiredInfoParticiple = 
    isMatch [
        Is "sloveso"
        Is "časování"
    ]

let hasRequiredInfoImperative = 
    ``match`` [
        Is "sloveso"
        Is "časování"
    ]
    >> Option.map getTables
    >> Option.map (Seq.map fst)
    >> Option.map (Seq.contains "Rozkazovací způsob")
    >> Option.contains true

let hasRequiredInfoConjugation = 
    isMatch [
        Is "sloveso"
        Is "časování"
    ]

let isValidVerb = isModern

let isParticipleValid word = 
    word |> isValidVerb &&
    word |> hasRequiredInfoParticiple

let isImperativeValid word =
    word |> isValidVerb &&
    word |> hasRequiredInfoImperative

let isConjugationValid word =
    word |> isValidVerb &&
    word |> hasRequiredInfoConjugation
