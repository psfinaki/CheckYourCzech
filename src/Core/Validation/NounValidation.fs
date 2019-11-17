module NounValidation

open Reflexives
open GrammarCategories
open Article
open NounArticle
open Genders
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

let hasRequiredInfo word =
    word
    |> isMatch [
        Is "čeština"
        Is "podstatné jméno"
        Starts "skloňování"
    ]

    &&

    word
    |> ``match`` [
        Is "čeština"
        Is "podstatné jméno"
    ] 
    |> Option.exists (hasInfo (Starts "rod"))

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

