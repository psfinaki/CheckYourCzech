module WikiDeclensions

open FSharp.Data
open GrammarCategories
open Article
open StringHelper

type EditableArticleOneDeclension = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">
type EditableArticleTwoDeclensions = HtmlProvider<"https://cs.wiktionary.org/wiki/čtvrt">
type LockedArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/debil">

let getUrl = (+) "https://cs.wiktionary.org/wiki/"

let getNumberOfDeclensions =
    getContent
    >> getChildPart "čeština"
    >> getChildPart "podstatné jméno"
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
