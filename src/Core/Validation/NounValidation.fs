module NounValidation

open Reflexives
open GrammarCategories
open Article
open NounArticle
open Genders
open StringHelper
open Nominalization

let isGenderValid = 
    getGender
    >> tryTranslateGender
    >> Option.isSome

let hasSingleDeclension case number = 
    getDeclension case number
    >> Seq.hasOneElement

let hasDeclension case number = 
    getDeclension case number
    >> Seq.any

let hasRequiredInfo =
    let hasDeclension = hasChildrenPartsWhen (starts "skloňování")
    let hasGender = hasInfo "rod"

    tryGetContent
    >> Option.filter (hasChildPart "čeština")
    >> Option.map (getChildPart "čeština")
    >> Option.filter (hasChildPart "podstatné jméno")
    >> Option.map (getChildPart "podstatné jméno")
    >> Option.filter hasDeclension
    >> Option.filter hasGender
    >> Option.isSome

let isValidNoun word =
    word |> hasRequiredInfo &&
    word |> isGenderValid &&
    word |> (not << isNominalization) &&
    word |> (not << isReflexive) &&
    word |> hasSingleDeclension Case.Nominative Number.Singular 

let isPluralValid word = 
    word |> isValidNoun &&
    word |> hasDeclension Case.Nominative Number.Plural

let isAccusativeValid word = 
    word |> isValidNoun &&
    word |> hasDeclension Case.Accusative Number.Singular

