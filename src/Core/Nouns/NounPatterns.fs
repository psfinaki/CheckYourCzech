module Core.Nouns.NounPatterns

open Common.GrammarCategories

let patternsGenderMap =
    dict [ (MasculineAnimate, MasculineAnimateNounPatternDetector.getPatterns)
           (MasculineInanimate, MasculineInanimateNounPatternDetector.getPatterns)
           (Feminine, FeminineNounPatternDetector.getPatterns)
           (Neuter, NeuterNounPatternDetector.getPatterns) ]

let getPatternsByGender declension gender = patternsGenderMap.[gender] declension

let getPatterns gender declension declinability = 
    match declinability with
    | Indeclinable ->
        Seq.empty
    | Declinable ->
        gender
        |> getPatternsByGender declension
