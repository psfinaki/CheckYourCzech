module Core.Nouns.NounPatterns

open Common.GenderTranslations
open Common.GrammarCategories
open WikiParsing.Articles.NounArticle

let patternsGenderMap =
    dict [ (MasculineAnimate, MasculineAnimateNounPatternDetector.getPatterns)
           (MasculineInanimate, MasculineInanimateNounPatternDetector.getPatterns)
           (Feminine, FeminineNounPatternDetector.getPatterns)
           (Neuter, NeuterNounPatternDetector.getPatterns) ]

let getPatternsByGender declension gender = patternsGenderMap.[gender] declension

let getPatterns article = 
    match article |> getDeclinability with
    | Indeclinable ->
        Seq.empty
    | Declinable ->
        article
        |> getGender
        |> Option.map fromString
        |> Option.map (getPatternsByGender (article |> getDeclension))
        |> Option.defaultValue Seq.empty
