module AdjectivePlural

open Microsoft.WindowsAzure.Storage.Table

type AdjectivePlural(word: string,
                     singular: string,
                     plural: string) =

    inherit TableEntity(word, word)
    
    new() = AdjectivePlural(null, null, null)

    member val Singular = singular with get, set
    member val Plural = plural with get, set
