module VerbValidation

open Article
open Archaisms

let tryGetVerb =
    tryGetContent
    >> Option.filter (hasChildPart "čeština")
    >> Option.map (getChildPart "čeština")
    >> Option.filter (hasChildPart "sloveso")
    >> Option.map (getChildPart "sloveso")

let hasRequiredInfoParticiple = 
    tryGetVerb
    >> Option.exists (hasChildPart "časování")

let hasRequiredInfoImperative = 
    let hasImperative = 
        Option.filter (hasChildPart "časování")
        >> Option.map (getChildPart "časování")
        >> Option.map getTables
        >> Option.map (Seq.map fst)
        >> Option.map (Seq.contains "Rozkazovací způsob")
        >> Option.contains true

    tryGetVerb
    >> hasImperative

let isValidVerb = isModern

let isParticipleValid word = 
    word |> isValidVerb &&
    word |> hasRequiredInfoParticiple

let isImperativeValid word =
    word |> isValidVerb &&
    word |> hasRequiredInfoImperative
