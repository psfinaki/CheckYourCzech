module Declensions

open FSharp.Data
open Article
open WikiString
open ArticleParser

type EditableArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">
type LockedArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/debil">

type Case = 
    | Nominative = 0
    | Genitive = 1
    | Dative = 2
    | Accusative = 3
    | Vocative = 4
    | Locative = 5
    | Instrumental = 6

type Number =
    | Singular
    | Plural

let hasDeclension = 
    tryGetNoun
    >> Option.bind (tryGetChildPart "skloňování")
    >> Option.isSome

let isIndeclinable = 
    getContent
    >> getChildPart "čeština"
    >> getChildPart "podstatné jméno"
    >> getChildPart "skloňování"
    >> tryGetInfo "nesklonné"
    >> Option.isSome

let isNominalization (noun: string) =
    let adjectiveEndings = ['ý'; 'á'; 'é'; 'í']
    let nounEnding = Seq.last noun
    adjectiveEndings |> Seq.contains nounEnding

let isNotNominalization = not << isNominalization

let getUrl = (+) "https://cs.wiktionary.org/wiki/"

let getDeclensionWikiEditable case number word =
    let data = word |> getUrl |> EditableArticle.Load
    match number with
    | Singular ->
        data.Tables.``Skloňování[editovat]``.Rows.[case].singulár
    | Plural -> 
        data.Tables.``Skloňování[editovat]``.Rows.[case].plurál

let getDeclensionWikiLocked case number word =
    let data = word |> getUrl |> LockedArticle.Load
    match number with
    | Singular ->
        data.Tables.Skloňování.Rows.[case].singulár
    | Plural -> 
        data.Tables.Skloňování.Rows.[case].plurál

let getDeclensionWiki (case: Case) number word = 
    match word with
    | _ when word |> isIndeclinable ->
        word
    | _ when word |> isEditable ->
        getDeclensionWikiEditable (int case) number word
    | _ when word |> isLocked ->
        getDeclensionWikiLocked (int case) number word
    | word -> 
        invalidOp ("Odd word: " + word)

let getDeclension case number = getDeclensionWiki case number >> getForms

let hasSingleDeclensionForCase case number = 
    getDeclension case number
    >> Seq.hasOneElement

let hasDeclensionForCase case number = 
    getDeclension case number
    >> Seq.any
