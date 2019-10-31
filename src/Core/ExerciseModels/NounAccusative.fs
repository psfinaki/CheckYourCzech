module NounAccusative

open Microsoft.WindowsAzure.Storage.Table
open GrammarCategories
open Noun
open Storage

type NounAccusative(word) =
    inherit TableEntity(word, word)

    let getNominative = map id serializeObject ""
    let getAccusatives = map (getDeclension Case.Accusative Number.Singular) serializeObject ""
    let getGender = map getGender serializeString ""
    let getPatterns = map getPatterns serializeObject ""

    new() = NounAccusative null

    member val Nominative = getNominative word with get, set
    member val Accusatives = getAccusatives word with get, set
    member val Gender = getGender word with get, set
    member val Patterns = getPatterns word with get, set
