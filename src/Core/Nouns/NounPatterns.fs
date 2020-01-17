module NounPatterns

open GenderTranslations
open NounArticle
open GrammarCategories

let patternsGenderMap =
    dict [ (MasculineAnimate, MasculineAnimateNounPatternDetector.getPatterns)
           (MasculineInanimate, MasculineInanimateNounPatternDetector.getPatterns)
           (Feminine, FeminineNounPatternDetector.getPatterns)
           (Neuter, NeuterNounPatternDetector.getPatterns) ]

let getPatternsByGender word gender = patternsGenderMap.[gender] word

let getPatterns noun = 
    match noun |> getDeclinability with
    | Indeclinable ->
        Seq.empty
    | Declinable ->
        noun
        |> getGender
        |> Option.map fromString
        |> Option.map (getPatternsByGender noun)
        |> Option.defaultValue Seq.empty
