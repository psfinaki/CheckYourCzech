module AdjectivePlural

open Microsoft.WindowsAzure.Storage.Table

let isValid word = 
    word |> Word.isAdjective

let getSingular = Storage.mapSafeString id
let getPlural = Storage.mapSafeString Adjective.getPlural

type AdjectivePlural(word) =
    inherit TableEntity(word, word)
    
    new() = AdjectivePlural null

    member val Singular = getSingular word with get, set
    member val Plural = getPlural word with get, set

let record word =
    if 
        isValid word
    then 
        word |> AdjectivePlural |> Storage.upsert "adjectiveplurals"
