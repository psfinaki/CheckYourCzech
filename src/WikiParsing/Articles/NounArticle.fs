module NounArticle

open FSharp.Data
open StringHelper
open GrammarCategories
open WikiString
open Article

type EditableArticleOneDeclension = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">
type EditableArticleTwoDeclensions = HtmlProvider<"https://cs.wiktionary.org/wiki/čtvrt">
type LockedArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/debil">

let getNoun =
    getContent
    >> getChildPart "čeština"
    >> getChildPart "podstatné jméno"

let getNumberOfDeclensions =
    getNoun
    >> getChildrenPartsWhen (starts "skloňování")
    >> Seq.length

let getEditable case number word =
    let numberOfDeclensions = getNumberOfDeclensions word
    let url = getUrl word
    match numberOfDeclensions with
    | 1 ->
        let data = EditableArticleOneDeclension.Load url
        match number with
        | Singular ->
            [ data.Tables.``Skloňování[editovat]``.Rows.[case].singulár ]
        | Plural -> 
            [ data.Tables.``Skloňování[editovat]``.Rows.[case].plurál ]
    | 2 ->
        let data = EditableArticleTwoDeclensions.Load url
        match number with
        | Singular ->
            [ data.Tables.``Skloňování (1)[editovat]``.Rows.[case].singulár 
              data.Tables.``Skloňování (2)[editovat]``.Rows.[case].singulár ]
        | Plural -> 
            [ data.Tables.``Skloňování (1)[editovat]``.Rows.[case].plurál 
              data.Tables.``Skloňování (2)[editovat]``.Rows.[case].plurál ]
    | _ ->
        invalidOp ("Odd word: " + word)

let getLocked case number word =
    let data = word |> getUrl |> LockedArticle.Load
    match number with
    | Singular ->
        [ data.Tables.Skloňování.Rows.[case].singulár ]
    | Plural -> 
        [ data.Tables.Skloňování.Rows.[case].plurál ]

let getDeclinability word = 
    let hasIndeclinabilityMarkInNounSection = 
        getNoun
        >> hasInfo "nesklonné"

    let hasIndeclinabilityMarkInDeclensionSections =
        getNoun
        >> getChildrenPartsWhen (starts "skloňování")
        >> Seq.forall (hasInfo "nesklonné")

    if
        word |> hasIndeclinabilityMarkInNounSection || 
        word |> hasIndeclinabilityMarkInDeclensionSections
    then Indeclinable
    else Declinable

let getDeclensionWiki (case: Case) number word = 
    match word with
    | _ when getDeclinability word = Indeclinable ->
        [ word ]
    | _ when word |> isEditable ->
        getEditable (int case) number word
    | _ when word |> isLocked ->
        getLocked (int case) number word
    | word -> 
        invalidOp ("Odd word: " + word)

let getDeclension case number = 
    getDeclensionWiki case number 
    >> Seq.collect getForms
    >> Seq.distinct

let getGender =
    getNoun
    >> getInfo "rod "
