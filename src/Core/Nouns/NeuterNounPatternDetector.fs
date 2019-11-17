module NeuterNounPatternDetector

open NounArticle
open StringHelper
open GrammarCategories
open Letters

let isPatternMěsto word = 
    let nominatives = word |> getDeclension Case.Nominative Number.Singular
    let genitives = word |> getDeclension Case.Genitive Number.Plural
    
    nominatives |> Seq.exists (ends "o") &&
    genitives |> Seq.exists (Stem.endsConsonant)

let isPatternMoře word =
    let singulars = word |> getDeclension Case.Nominative Number.Singular
    let plurals = word |> getDeclension Case.Nominative Number.Plural

    singulars |> Seq.exists (endsOneOf ["e"; "ě"]) &&
    plurals |> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternStavení =
    getDeclension Case.Genitive Number.Singular
    >> Seq.exists (ends "í")

let isPatternKuře word =
    let singulars = word |> getDeclension Case.Nominative Number.Singular
    let plurals = word |> getDeclension Case.Nominative Number.Plural

    singulars |> Seq.exists (endsOneOf ["e"; "ě"]) &&
    plurals |> Seq.exists (ends "ata")

let isPatternDrama word =
    let singulars = word |> getDeclension Case.Nominative Number.Singular
    let plurals = word |> getDeclension Case.Nominative Number.Plural

    singulars |> Seq.exists (ends "ma") &&
    plurals |> Seq.exists (ends "mata")

let isPatternMuzeum word =
    let stemEndsVowel = remove "um" >> Seq.last >> isVowel
    let rule word = word |> ends "um" && word |> stemEndsVowel

    let singulars = word |> getDeclension Case.Nominative Number.Singular
    singulars |> Seq.exists rule

let patternDetectors = [
    (isPatternMěsto, "město")
    (isPatternMoře, "moře")
    (isPatternStavení, "stavení")
    (isPatternKuře, "kuře")
    (isPatternDrama, "drama")
    (isPatternMuzeum, "muzeum")
]

let isPattern word patternDetector = fst patternDetector word

let getPatterns word = 
    patternDetectors
    |> Seq.where (isPattern word)
    |> Seq.map snd
