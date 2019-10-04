module NounAccusative

open Microsoft.WindowsAzure.Storage.Table

let isValid word = 
    word |> Word.isNoun &&
    word |> Noun.isNotNominalization &&
    word |> Noun.hasDeclension &&
    word |> Noun.hasGender &&
    word |> Declensions.hasSingleDeclensionForCase Declensions.Case.Nominative Declensions.Number.Singular

let getNominative = Storage.mapSafeString id
let getAccusatives = Storage.mapSafeString (Declensions.getDeclension Declensions.Case.Accusative Declensions.Number.Singular)
let getGender = Storage.mapSafeObject (Noun.getGender >> box)
let getPatterns = Storage.mapSafeString Noun.getPatterns

type NounAccusative(word) =
    inherit TableEntity(word, word)
    
    new() = NounAccusative null

    member val Nominative = getNominative word with get, set
    member val Gender = getGender word with get, set
    member val Patterns = getPatterns word with get, set
    member val Accusatives = getAccusatives word with get, set

let record word =
    if 
        isValid word
    then
        word |> NounAccusative |> Storage.upsert "nounaccusatives"
