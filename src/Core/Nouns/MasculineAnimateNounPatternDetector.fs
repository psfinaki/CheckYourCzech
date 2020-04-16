module MasculineAnimateNounPatternDetector

open NounArticle
open StringHelper
open GrammarCategories

let isPatternPán = 
    getDeclension Case.Genitive Number.Singular
    >> Seq.exists (ends "a")

let isPatternMuž article =
    let nominatives = article |> getDeclension Case.Nominative Number.Singular
    let genitives = article |> getDeclension Case.Genitive Number.Singular

    nominatives |> Seq.exists (Stem.endsSoft) &&
    genitives |> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternPředseda =
    getDeclension Case.Nominative Number.Singular
    >> Seq.exists (ends "a")

let isPatternSoudce =
    getDeclension Case.Nominative Number.Singular
    >> Seq.exists (ends "ce")

let patternDetectors = [
    (isPatternPán, "pán")
    (isPatternMuž, "muž")
    (isPatternPředseda, "předseda")
    (isPatternSoudce, "soudce")
]

let isPattern article patternDetector = fst patternDetector article

let getPatterns article = 
    patternDetectors
    |> Seq.where (isPattern article)
    |> Seq.map snd
