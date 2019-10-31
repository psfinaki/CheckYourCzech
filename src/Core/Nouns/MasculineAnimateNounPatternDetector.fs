module MasculineAnimateNounPatternDetector

open NounArticle
open StringHelper
open GrammarCategories

let isPatternPan = 
    getDeclension Case.Genitive Number.Singular
    >> Seq.exists (ends "a")

let isPatternMuž noun =
    let nominatives = noun |> getDeclension Case.Nominative Number.Singular
    let genitives = noun |> getDeclension Case.Genitive Number.Singular

    nominatives |> Seq.exists (Stem.endsSoft) &&
    genitives |> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternPředseda =
    getDeclension Case.Nominative Number.Singular
    >> Seq.exists (ends "a")

let isPatternSoudce =
    getDeclension Case.Nominative Number.Singular
    >> Seq.exists (ends "ce")

let patternDetectors = [
    (isPatternPan, "pan")
    (isPatternMuž, "muž")
    (isPatternPředseda, "předseda")
    (isPatternSoudce, "soudce")
]

let isPattern word patternDetector = fst patternDetector word

let getPatterns word = 
    patternDetectors
    |> Seq.where (isPattern word)
    |> Seq.map snd
