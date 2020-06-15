module Core.Nouns.NounPatterns

open Common.GenderTranslations
open Common.GrammarCategories
open WikiParsing.Articles.NounArticle

let patternsGenderMap =
    dict [ (MasculineAnimate, MasculineAnimateNounPatternDetector.getPatterns)
           (MasculineInanimate, MasculineInanimateNounPatternDetector.getPatterns)
           (Feminine, FeminineNounPatternDetector.getPatterns)
           (Neuter, NeuterNounPatternDetector.getPatterns) ]

let getPatternsByGender word gender = patternsGenderMap.[gender] word

let getPatterns article = 
    match article |> getDeclinability with
    | Indeclinable ->
        Seq.empty
    | Declinable ->
        article
        |> getGender
        |> Option.map fromString
        |> Option.map (getPatternsByGender article)
        |> Option.defaultValue Seq.empty
