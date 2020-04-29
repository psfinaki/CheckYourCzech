module NounArticle

open FSharp.Data
open GrammarCategories
open WikiString
open Article
open Common.Utils
open GenderTranslations
open WikiArticles

type EditableArticleOneDeclension = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">
type EditableArticleTwoDeclensions = HtmlProvider<"https://cs.wiktionary.org/wiki/čtvrt">
type LockedArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/debil">

let getNumberOfDeclensions (NounArticle article) =
    article
    |> matches [
        Is "podstatné jméno"
        Starts "skloňování"
    ]
    |> Seq.length

let getEditable case number article =
    let numberOfDeclensions = getNumberOfDeclensions article
    let (NounArticle {Title = word; Text = text}) = article

    match numberOfDeclensions with
    | 1 ->
        let data = EditableArticleOneDeclension.Parse text
        match number with
        | Singular ->
            [ data.Tables.``Skloňování[editovat]``.Rows.[case].singulár ]
        | Plural -> 
            [ data.Tables.``Skloňování[editovat]``.Rows.[case].plurál ]
    | 2 ->
        let data = EditableArticleTwoDeclensions.Parse text
        match number with
        | Singular ->
            [ data.Tables.``Skloňování (1)[editovat]``.Rows.[case].singulár 
              data.Tables.``Skloňování (2)[editovat]``.Rows.[case].singulár ]
        | Plural -> 
            [ data.Tables.``Skloňování (1)[editovat]``.Rows.[case].plurál 
              data.Tables.``Skloňování (2)[editovat]``.Rows.[case].plurál ]
    | _ ->
        invalidOp ("Odd word: " + word)

let getLocked case number (NounArticle { Text = text }) =
    let data = LockedArticle.Parse text
    match number with
    | Singular ->
        [ data.Tables.Skloňování.Rows.[case].singulár ]
    | Plural -> 
        [ data.Tables.Skloňování.Rows.[case].plurál ]

let getDeclinability (NounArticle article) =
    let hasIndeclinabilityMarkInNounSection = 
        ``match`` [
            Is "podstatné jméno"
        ]
        >> Option.exists (hasInfo (Is "nesklonné"))

    let hasIndeclinabilityMarkInDeclensionSections =
        matches [
            Is "podstatné jméno"
            Starts "skloňování"
        ]
        >> Seq.exists (hasInfo (Is "nesklonné"))

    if
        article |> hasIndeclinabilityMarkInNounSection || 
        article |> hasIndeclinabilityMarkInDeclensionSections
    then Indeclinable
    else Declinable

let getDeclensionWiki (case: Case) number nounArticle =
    let (NounArticle article) = nounArticle
    let word = article.Title

    match word with
    | _ when getDeclinability nounArticle = Indeclinable ->
        [ word ]
    | _ when article |> isEditable ->
        getEditable (int case) number nounArticle
    | _ when article |> isLocked ->
        getLocked (int case) number nounArticle
    | word -> 
        invalidOp ("Odd word: " + word)

let getDeclension case number = 
    getDeclensionWiki case number 
    >> Seq.collect getForms
    >> Seq.distinct

let getGender (NounArticle article) =
    article
    |> ``match`` [
        Is "podstatné jméno"
    ] 
    |> Option.map (getInfos (OneOf (getAllUnion<Gender> |> Seq.map toString)))
    |> Option.filter Seq.hasOneElement
    |> Option.map Seq.exactlyOne
