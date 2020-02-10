module NounPlural

open Microsoft.WindowsAzure.Storage.Table

type NounPlural(word: string, 
                singular: string,
                plurals: string, 
                gender: string, 
                patterns: string) =

    inherit TableEntity(word, word)
    
    new() = NounPlural(null, null, null, null, null)

    member val Singular = singular with get, set
    member val Plurals = plurals with get, set
    member val Gender = gender with get, set
    member val Patterns = patterns with get, set
