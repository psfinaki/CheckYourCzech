module MasculineInanimateNounPatternDetector

open NounArticle
open StringHelper
open GrammarCategories

let canBeLoanword = endsOneOf ["us"; "es"; "os"]

let isPatternHrad = function
    | noun when noun |> canBeLoanword -> 
        let singulars = noun |> getDeclension Case.Nominative Number.Singular
        let plurals = noun |> getDeclension Case.Nominative Number.Plural
        
        let isPluralPatternHrad (singular, plural) = 
            singular |> append "y" = plural

        Seq.allPairs singulars plurals |> Seq.exists isPluralPatternHrad
    | noun ->
        noun
        |> getDeclension Case.Genitive Number.Singular
        |> Seq.exists (endsOneOf ["u"; "a"])

let isPatternStroj =
    getDeclension Case.Nominative Number.Plural
    >> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternRytmus noun =
    noun |> canBeLoanword && 

    let singulars = noun |> getDeclension Case.Nominative Number.Singular
    let plurals = noun |> getDeclension Case.Nominative Number.Plural

    let isPluralPatternRytmus (singular, plural) = 
        singular |> removeLast 2 |> append "y" = plural

    Seq.allPairs singulars plurals |> Seq.exists isPluralPatternRytmus

let patternDetectors = [
    (isPatternHrad, "hrad")
    (isPatternStroj, "stroj")
    (isPatternRytmus, "rytmus")
]

let isPattern word patternDetector = fst patternDetector word

let getPatterns word = 
    patternDetectors
    |> Seq.where (isPattern word)
    |> Seq.map snd
