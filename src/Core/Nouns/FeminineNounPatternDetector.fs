module Core.Nouns.FeminineNounPatternDetector

open Common.StringHelper
open Common.GrammarCategories
open Core.Stem

let isPatternŽena declension = 
    let nominatives = declension.SingularNominative
    let genitives = declension.SingularGenitive

    nominatives |> Seq.exists (ends "a") && 
    genitives |> Seq.exists (endsOneOf ["y"; "i"])

let isPatternRůže declension =
    let nominatives = declension.SingularNominative
    let genitives = declension.SingularGenitive

    nominatives |> Seq.exists (endsOneOf ["e"; "ě"; "a"]) && 
    genitives |> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternPíseň declension =
    let nominatives = declension.SingularNominative
    let genitives = declension.SingularGenitive

    nominatives |> Seq.exists endsConsonant &&
    genitives |> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternKost declension =
    let datives = declension.PluralDative
    let instrumentals = declension.PluralInstrumental

    datives |> Seq.exists (ends "em") &&
    instrumentals |> Seq.exists (not << endsOneOf ["emi"; "ěmi"])

let patternDetectors = [
    (isPatternŽena, "žena")
    (isPatternRůže, "růže")
    (isPatternPíseň, "píseň")
    (isPatternKost, "kost")
]

let isPattern article patternDetector = fst patternDetector article

let getPatterns article = 
    patternDetectors
    |> Seq.where (isPattern article)
    |> Seq.map snd
