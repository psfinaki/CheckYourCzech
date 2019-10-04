module FeminineNounPatternDetector

open Declensions
open StringHelper
open NounCategories

let isPatternŽena word = 
    let nominatives = word |> getDeclension Case.Nominative Number.Singular
    let genitives = word |> getDeclension Case.Genitive Number.Singular

    nominatives |> Seq.exists (ends "a") && 
    genitives |> Seq.exists (endsOneOf ["y"; "i"])

let isPatternRůže word =
    let nominatives = word |> getDeclension Case.Nominative Number.Singular
    let genitives = word |> getDeclension Case.Genitive Number.Singular

    nominatives |> Seq.exists (endsOneOf ["e"; "ě"; "a"]) && 
    genitives |> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternPíseň word =
    let nominatives = word |> getDeclension Case.Nominative Number.Singular
    let genitives = word |> getDeclension Case.Genitive Number.Singular

    nominatives |> Seq.exists Stem.endsConsonant &&
    genitives |> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternKost word =
    let datives = word |> getDeclension Case.Dative Number.Plural
    let instrumentals = word |> getDeclension Case.Instrumental Number.Plural

    datives |> Seq.exists (ends "em") &&
    instrumentals |> Seq.exists (not << endsOneOf ["emi"; "ěmi"])

let patternDetectors = [
    (isPatternŽena, "žena")
    (isPatternRůže, "růže")
    (isPatternPíseň, "píseň")
    (isPatternKost, "kost")
]

let isPattern word patternDetector = fst patternDetector word

let getPatterns word = 
    patternDetectors
    |> Seq.where (isPattern word)
    |> Seq.map snd
