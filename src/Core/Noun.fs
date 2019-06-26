module Noun

open FSharp.Data
open Article
open Genders
open ArticleParser

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

let patternsGenderMap =
    dict [ (MasculineAnimate, MasculineAnimateNounPatternDetector.getPatterns)
           (MasculineInanimate, MasculineInanimateNounPatternDetector.getPatterns)
           (Feminine, FeminineNounPatternDetector.getPatterns)
           (Neuter, NeuterNounPatternDetector.getPatterns) ]

let getPatternsByGender word gender = patternsGenderMap.[gender] word

let getPatterns noun =
    noun
    |> getGender
    |> getPatternsByGender noun
