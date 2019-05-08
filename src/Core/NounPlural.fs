module NounPlural

open Microsoft.WindowsAzure.Storage.Table

let isValid word = 
    word |> Word.isNoun &&
    word |> Noun.hasDeclension &&
    word |> Noun.hasGender &&
    word |> Noun.hasSingleDeclensionForCase Noun.Case.Nominative Noun.Number.Singular &&
    word |> Noun.hasDeclensionForCase Noun.Case.Nominative Noun.Number.Plural

let getSingular = Storage.mapSafeString id
let getPlurals = Storage.mapSafeString (Noun.getDeclensionForCase Noun.Case.Nominative Noun.Number.Plural)
let getGender = Storage.mapSafeObject (Noun.getGender >> box)
let getPattern = Storage.mapSafeStringOption Noun.getPattern   

type Plural(word) =
    inherit TableEntity(word, word)
    
    new() = Plural null

    member val Singular = getSingular word with get, set
    member val Plurals = getPlurals word with get, set
    member val Gender = getGender word with get, set
    member val Pattern = getPattern word with get, set

let record word =
    if 
        isValid word
    then
        word |> Plural |> Storage.upsert "plurals"
