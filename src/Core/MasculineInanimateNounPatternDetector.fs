module MasculineInanimateNounPatternDetector

open Declensions
open StringHelper

let isPatternHrad = 
    getDeclension Case.Genitive Number.Singular
    >> Seq.exists (endsOneOf ["u"; "a"])

let isPatternStroj =
    getDeclension Case.Nominative Number.Plural
    >> Seq.exists (endsOneOf ["e"; "ě"])

let patternDetectors = [
    (isPatternHrad, "hrad")
    (isPatternStroj, "stroj")
]

let isPattern word patternDetector = fst patternDetector word

let getPatterns word = 
    patternDetectors
    |> Seq.where (isPattern word)
    |> Seq.map snd
