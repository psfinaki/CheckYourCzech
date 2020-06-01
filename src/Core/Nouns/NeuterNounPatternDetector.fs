module Core.Nouns.NeuterNounPatternDetector

open WikiParsing.Articles.NounArticle
open Common.StringHelper
open Common.GrammarCategories
open Core.Letters
open Core.Stem

let isPatternMěsto article = 
    let nominatives = article |> getDeclension Case.Nominative Number.Singular
    let genitives = article |> getDeclension Case.Genitive Number.Plural
    
    nominatives |> Seq.exists (ends "o") &&
    genitives |> Seq.exists (endsConsonant)

let isPatternMoře article =
    let singulars = article |> getDeclension Case.Nominative Number.Singular
    let plurals = article |> getDeclension Case.Nominative Number.Plural

    singulars |> Seq.exists (endsOneOf ["e"; "ě"]) &&
    plurals |> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternStavení =
    getDeclension Case.Genitive Number.Singular
    >> Seq.exists (ends "í")

let isPatternKuře article =
    let singulars = article |> getDeclension Case.Nominative Number.Singular
    let plurals = article |> getDeclension Case.Nominative Number.Plural

    singulars |> Seq.exists (endsOneOf ["e"; "ě"]) &&
    plurals |> Seq.exists (ends "ata")

let isPatternDrama article =
    let singulars = article |> getDeclension Case.Nominative Number.Singular
    let plurals = article |> getDeclension Case.Nominative Number.Plural

    singulars |> Seq.exists (ends "ma") &&
    plurals |> Seq.exists (ends "mata")

let isPatternMuzeum article =
    let stemEndsVowel = remove "um" >> Seq.last >> isVowel
    let rule word = word |> ends "um" && word |> stemEndsVowel

    let singulars = article |> getDeclension Case.Nominative Number.Singular
    singulars |> Seq.exists rule

let patternDetectors = [
    (isPatternMěsto, "město")
    (isPatternMoře, "moře")
    (isPatternStavení, "stavení")
    (isPatternKuře, "kuře")
    (isPatternDrama, "drama")
    (isPatternMuzeum, "muzeum")
]

let isPattern article patternDetector = fst patternDetector article

let getPatterns article = 
    patternDetectors
    |> Seq.where (isPattern article)
    |> Seq.map snd
