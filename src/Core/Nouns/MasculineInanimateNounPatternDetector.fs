module Core.Nouns.MasculineInanimateNounPatternDetector

open WikiParsing.Articles.NounArticle
open Common.StringHelper
open Common.GrammarCategories
open Common.WikiArticles

let canBeLoanword = endsOneOf ["us"; "es"; "os"]

let isPatternHrad article = 
    let (NounArticle { Title = noun }) = article
    match article with
    | article when noun |> canBeLoanword -> 
        let singulars = article |> getDeclension Case.Nominative Number.Singular
        let plurals = article |> getDeclension Case.Nominative Number.Plural
        
        let isPluralPatternHrad (singular, plural) = 
            singular |> append "y" = plural

        Seq.allPairs singulars plurals |> Seq.exists isPluralPatternHrad
    | article ->
        article
        |> getDeclension Case.Genitive Number.Singular
        |> Seq.exists (endsOneOf ["u"; "a"])

let isPatternStroj =
    getDeclension Case.Nominative Number.Plural
    >> Seq.exists (endsOneOf ["e"; "ě"])

let isPatternRytmus article =
    let (NounArticle { Title = noun }) = article

    noun |> canBeLoanword && 

    let singulars = article |> getDeclension Case.Nominative Number.Singular
    let plurals = article |> getDeclension Case.Nominative Number.Plural

    let isPluralPatternRytmus (singular, plural) = 
        singular |> removeLast 2 |> append "y" = plural

    Seq.allPairs singulars plurals |> Seq.exists isPluralPatternRytmus

let patternDetectors = [
    (isPatternHrad, "hrad")
    (isPatternStroj, "stroj")
    (isPatternRytmus, "rytmus")
]

let isPattern article patternDetector = fst patternDetector article

let getPatterns article = 
    patternDetectors
    |> Seq.where (isPattern article)
    |> Seq.map snd
