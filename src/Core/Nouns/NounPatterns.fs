module Core.Nouns.NounPatterns

open Common.GrammarCategories.Nouns

let getPatternsByGender declension = function
    | Gender.MasculineAnimate -> 
        declension 
        |> MasculineAnimateNounPatternDetector.getPatterns 
        |> Seq.map DeclensionPattern.MasculineAnimate
    | Gender.MasculineInanimate -> 
        declension 
        |> MasculineInanimateNounPatternDetector.getPatterns 
        |> Seq.map DeclensionPattern.MasculineInanimate
    | Gender.Feminine -> 
        declension 
        |> FeminineNounPatternDetector.getPatterns 
        |> Seq.map DeclensionPattern.Feminine
    | Gender.Neuter -> 
        declension 
        |> NeuterNounPatternDetector.getPatterns 
        |> Seq.map DeclensionPattern.Neuter

let getPatterns gender declension declinability = 
    match declinability with
    | Indeclinable ->
        Seq.empty
    | Declinable ->
        gender
        |> getPatternsByGender declension
