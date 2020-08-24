module Core.Nouns.NeuterNounPatternDetector

open Common.StringHelper
open Common.GrammarCategories.Nouns
open Core.Letters
open Core.Stem

let isPatternMěsto declension = 
    let nominatives = declension.SingularNominative
    let genitives = declension.PluralGenitive
    
    nominatives |> Seq.exists (ends "o") &&
    genitives |> Seq.exists (endsConsonant)

let isPatternMoře declension =
    let singulars = declension.SingularNominative
    let plurals = declension.PluralNominative

    singulars |> Seq.exists (endsOneOf ["e"; "ě"]) &&
    plurals |> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternStavení declension =
    declension.SingularGenitive
    |> Seq.exists (ends "í")

let isPatternKuře declension =
    let singulars = declension.SingularNominative
    let plurals = declension.PluralNominative

    singulars |> Seq.exists (endsOneOf ["e"; "ě"]) &&
    plurals |> Seq.exists (ends "ata")

let isPatternDrama declension =
    let singulars = declension.SingularNominative
    let plurals = declension.PluralNominative

    singulars |> Seq.exists (ends "ma") &&
    plurals |> Seq.exists (ends "mata")

let isPatternMuzeum declension =
    let stemEndsVowel = remove "um" >> Seq.last >> isVowel
    let rule word = word |> ends "um" && word |> stemEndsVowel

    let singulars = declension.SingularNominative
    singulars |> Seq.exists rule

let patternDetectors = [
    (isPatternMěsto, NeuterDeclensionPattern.Město)
    (isPatternMoře, NeuterDeclensionPattern.Moře)
    (isPatternStavení, NeuterDeclensionPattern.Stavení)
    (isPatternKuře, NeuterDeclensionPattern.Kuře)
    (isPatternDrama, NeuterDeclensionPattern.Drama)
    (isPatternMuzeum, NeuterDeclensionPattern.Muzeum)
]

let isPattern article patternDetector = fst patternDetector article

let getPatterns article = 
    patternDetectors
    |> Seq.where (isPattern article)
    |> Seq.map snd
