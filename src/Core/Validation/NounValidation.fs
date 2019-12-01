module NounValidation

open Reflexives
open GrammarCategories
open Article
open NounArticle
open GenderTranslations
open Nominalization
open Common.Utils

let hasSingleDeclension case number = 
    getDeclension case number
    >> Seq.hasOneElement

let hasDeclension case number = 
    getDeclension case number
    >> Seq.any

let hasRequiredInfo word =
    word
    |> isMatch [
        Is "podstatné jméno"
        Starts "skloňování"
    ]

    &&

    word
    |> ``match`` [
        Is "podstatné jméno"
    ] 
    |> Option.exists (hasInfo (OneOf (getAllUnion<Gender> |> Seq.map toString)))

let isValidNoun word =
    word |> hasRequiredInfo &&
    word |> (not << isNominalization) &&
    word |> (not << isReflexive) &&
    word |> hasSingleDeclension Case.Nominative Number.Singular 

let isPluralValid word = 
    word |> isValidNoun &&
    word |> hasDeclension Case.Nominative Number.Plural

let isAccusativeValid word = 
    word |> isValidNoun &&
    word |> hasDeclension Case.Accusative Number.Singular

