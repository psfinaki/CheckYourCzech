module Noun

open FSharp.Data
open Article
open Genders
open ArticleParser
open Declensions

type EditableArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">
type LockedArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/debil">

let hasDeclension = 
    tryGetNoun
    >> Option.bind (tryGetChildPart "skloňování")
    >> Option.isSome

let hasGender =
    tryGetNoun
    >> Option.bind (tryGetInfo "rod")
    >> Option.bind tryTranslateGender
    >> Option.isSome

let getGender =
    getContent
    >> getChildPart "čeština"
    >> getChildPart "podstatné jméno"
    >> getInfo "rod "
    >> translateGender

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

let getPattern noun = 
    let nominatives = getDeclension Case.Nominative Number.Singular noun
    let isPatternDetectionPossible = Seq.hasOneElement

    if 
        isPatternDetectionPossible nominatives
    then
        let gender = getGender noun
        let nominative = nominatives |> Seq.exactlyOne
        let genitives = getDeclension Case.Genitive Number.Singular noun

        genitives
        |> Seq.map (NounPatternDetector.getPatternByGender gender nominative)
        |> Seq.choose id
        |> Seq.distinct
        |> Seq.tryExactlyOne
    else 
        None
