module NounPlural

open Microsoft.WindowsAzure.Storage.Table

let isValid word = 
    word |> Word.isNoun &&
    word |> Noun.isNotNominalization &&
    word |> (not << Noun.hasParticles) &&
    word |> Noun.hasDeclension &&
    word |> Noun.hasGender &&
    word |> Declensions.hasSingleDeclensionForCase GrammarCategories.Case.Nominative GrammarCategories.Number.Singular &&
    word |> Declensions.hasDeclensionForCase GrammarCategories.Case.Nominative GrammarCategories.Number.Plural

let getSingular = Storage.mapSafeString id
let getPlurals = Storage.mapSafeString (Declensions.getDeclension GrammarCategories.Case.Nominative GrammarCategories.Number.Plural)
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
