module NounPlural

open Microsoft.WindowsAzure.Storage.Table
open GrammarCategories
open Noun
open Storage

type NounPlural(word) =
    inherit TableEntity(word, word)
    
    let getSingular = map id serializeObject ""
    let getPlurals = map (getDeclension Case.Nominative Number.Plural) serializeObject ""
    let getGender = map getGender serializeString ""
    let getPatterns = map getPatterns serializeObject ""

    new() = NounPlural null

    member val Singular = getSingular word with get, set
    member val Plurals = getPlurals word with get, set
    member val Gender = getGender word with get, set
    member val Patterns = getPatterns word with get, set
