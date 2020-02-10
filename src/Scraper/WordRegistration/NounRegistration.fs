module NounRegistration

open Storage
open Noun
open GrammarCategories

let registerNounPlural word =
    let singular = word |> map id serializeObject ""
    let plurals = word |> map (getDeclension Case.Nominative Number.Plural) serializeObject ""
    let gender = word |> map getGender serializeOption<Gender> ""
    let patterns = word |> map getPatterns serializeObject ""

    NounPlural.NounPlural(word, singular, plurals, gender, patterns)
    |> upsert "nounplurals"

let registerNounAccusative word =
    let nominative = word |> map id serializeObject ""
    let accusatives = word |> map (getDeclension Case.Accusative Number.Singular) serializeObject ""
    let gender = word |> map getGender serializeOption<Gender> ""
    let patterns = word |> map getPatterns serializeObject ""
    NounAccusative.NounAccusative(word, nominative, accusatives, gender, patterns)
    |> upsert "nounaccusatives"
