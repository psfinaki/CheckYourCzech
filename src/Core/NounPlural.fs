module NounPlural

open Microsoft.WindowsAzure.Storage.Table

let isValid word = 
    word |> Word.isNoun &&
    word |> Noun.isNotNominalization &&
    word |> (not << Noun.hasParticles) &&
    word |> Noun.hasDeclension &&
    word |> Noun.hasGender &&
    word |> Declensions.hasSingleDeclensionForCase Declensions.Case.Nominative Declensions.Number.Singular &&
    word |> Declensions.hasDeclensionForCase Declensions.Case.Nominative Declensions.Number.Plural

let getSingular = Storage.mapSafeString id
let getPlurals = Storage.mapSafeString (Declensions.getDeclension Declensions.Case.Nominative Declensions.Number.Plural)
let getGender = Storage.mapSafeObject (Noun.getGender >> box)
let getPatterns = Storage.mapSafeString Noun.getPatterns

type NounPlural(word) =
    inherit TableEntity(word, word)
    
    new() = NounPlural null

    member val Singular = getSingular word with get, set
    member val Plurals = getPlurals word with get, set
    member val Gender = getGender word with get, set
    member val Patterns = getPatterns word with get, set

let record word =
    if 
        isValid word
    then
        word |> NounPlural |> Storage.upsert "nounplurals"
