module Core.Nouns.MasculineAnimateNounPatternDetector

open Common.StringHelper
open Common.Declension
open Core.Stem

let isPatternPán declension = 
    declension.SingularGenitive
    |> Seq.exists (ends "a")

let isPatternMuž declension =
    let nominatives = declension.SingularNominative
    let genitives = declension.SingularGenitive

    nominatives |> Seq.exists (endsSoft) &&
    genitives |> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternPředseda declension =
    declension.SingularNominative
    |> Seq.exists (ends "a")

let isPatternSoudce declension =
    declension.SingularNominative
    |> Seq.exists (ends "ce")

let patternDetectors = [
    (isPatternPán, Pán)
    (isPatternMuž, Muž)
    (isPatternPředseda, Předseda)
    (isPatternSoudce, Soudce)
]

let isPattern article patternDetector = fst patternDetector article

let getPatterns article = 
    patternDetectors
    |> Seq.where (isPattern article)
    |> Seq.map snd
