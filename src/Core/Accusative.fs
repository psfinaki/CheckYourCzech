module Accusative

open Microsoft.WindowsAzure.Storage.Table

let isValid word = 
    word |> Word.isNoun &&
    word |> Noun.hasDeclension &&
    word |> Noun.hasGender &&
    word |> Noun.hasSingleDeclensionForCase Noun.Case.Nominative Noun.Number.Singular

let getNominative = Storage.mapSafeString id
let getAccusatives = Storage.mapSafeString (Noun.getDeclensionForCase Noun.Case.Accusative Noun.Number.Singular)
let getGender = Storage.mapSafeObject (Noun.getGender >> box)
let getPattern = Storage.mapSafeStringOption Noun.getPattern 

type Accusative(word) =
    inherit TableEntity(word, word)
    
    new() = Accusative null

    member val Nominative = getNominative word with get, set
    member val Gender = getGender word with get, set
    member val Pattern = getPattern word with get, set
    member val Accusatives = getAccusatives word with get, set

let record word =
    if 
        isValid word
    then
        word |> Accusative |> Storage.upsert "accusatives"
