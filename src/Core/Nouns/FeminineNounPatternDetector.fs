module FeminineNounPatternDetector

open NounArticle
open StringHelper
open GrammarCategories

let isPatternŽena article = 
    let nominatives = article |> getDeclension Case.Nominative Number.Singular
    let genitives = article |> getDeclension Case.Genitive Number.Singular

    nominatives |> Seq.exists (ends "a") && 
    genitives |> Seq.exists (endsOneOf ["y"; "i"])

let isPatternRůže article =
    let nominatives = article |> getDeclension Case.Nominative Number.Singular
    let genitives = article |> getDeclension Case.Genitive Number.Singular

    nominatives |> Seq.exists (endsOneOf ["e"; "ě"; "a"]) && 
    genitives |> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternPíseň article =
    let nominatives = article |> getDeclension Case.Nominative Number.Singular
    let genitives = article |> getDeclension Case.Genitive Number.Singular

    nominatives |> Seq.exists Stem.endsConsonant &&
    genitives |> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternKost article =
    let datives = article |> getDeclension Case.Dative Number.Plural
    let instrumentals = article |> getDeclension Case.Instrumental Number.Plural

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
