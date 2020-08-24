module Core.Nouns.MasculineInanimateNounPatternDetector

open Common.StringHelper
open Common.GrammarCategories.Nouns

let canBeLoanword = endsOneOf ["us"; "es"; "os"]

let isPatternHrad declension = 
    if declension.SingularNominative |> Seq.exists canBeLoanword
    then
        let singulars = declension.SingularNominative
        let plurals = declension.PluralNominative

        let isPluralPatternHrad (singular, plural) = 
            singular |> append "y" = plural

        Seq.allPairs singulars plurals |> Seq.exists isPluralPatternHrad
    else
        declension.SingularGenitive
        |> Seq.exists (endsOneOf ["u"; "a"])

let isPatternStroj declension =
    declension.PluralNominative
    |> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternRytmus declension =
    declension.SingularNominative |> Seq.exists canBeLoanword && 

    let singulars = declension.SingularNominative
    let plurals = declension.PluralNominative

    let isPluralPatternRytmus (singular, plural) = 
        singular |> removeLast 2 |> append "y" = plural

    Seq.allPairs singulars plurals |> Seq.exists isPluralPatternRytmus

let patternDetectors = [
    (isPatternHrad, MasculineInanimateDeclensionPattern.Hrad)
    (isPatternStroj, MasculineInanimateDeclensionPattern.Stroj)
    (isPatternRytmus, MasculineInanimateDeclensionPattern.Rytmus)
]

let isPattern article patternDetector = fst patternDetector article

let getPatterns article = 
    patternDetectors
    |> Seq.where (isPattern article)
    |> Seq.map snd
