module Noun

open FSharp.Data
open Article
open Genders
open WikiString

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

let getGender =
    getContent
    >> getPart "čeština"
    >> getPart "podstatné jméno"
    >> getInfo "rod "
    >> translateGender

let isIndeclinable = 
    getContent
    >> getPart "čeština"
    >> getPart "podstatné jméno"
    >> getPart "skloňování"
    >> tryGetInfo "nesklonné"
    >> Option.isSome

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

let isPatternDetectionPossible (nominatives, genitives) = 
    nominatives |> Seq.hasOneElement && 
    genitives |> Seq.hasOneElement

let getPattern noun = 
    let nominatives = getDeclension Case.Nominative Number.Singular noun
    let genitives = getDeclension Case.Genitive Number.Singular noun

    if 
        isPatternDetectionPossible (nominatives, genitives)
    then
        let gender = getGender noun
        let nominative = nominatives |> Seq.exactlyOne
        let genitive = genitives |> Seq.exactlyOne
        NounPatternDetector.getPatternByGender gender (nominative, genitive)
    else 
        None
