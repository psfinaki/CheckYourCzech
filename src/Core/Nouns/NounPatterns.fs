module Core.Nouns.NounPatterns

open Common.Declension
open Common.GrammarCategories

let patternsGenderMap =
    dict [ (Gender.MasculineAnimate, MasculineAnimateNounPatternDetector.getPatterns >> Seq.map DeclensionPattern.MasculineAnimate)
           (Gender.MasculineInanimate, MasculineInanimateNounPatternDetector.getPatterns >> Seq.map DeclensionPattern.MasculineInanimate)
           (Gender.Feminine, FeminineNounPatternDetector.getPatterns >> Seq.map DeclensionPattern.Feminine)
           (Gender.Neuter, NeuterNounPatternDetector.getPatterns >> Seq.map DeclensionPattern.Neuter) ]

let getPatternsByGender declension gender = patternsGenderMap.[gender] declension

let getPatterns gender declension declinability = 
    match declinability with
    | Indeclinable ->
        Seq.empty
    | Declinable ->
        gender
        |> getPatternsByGender declension
