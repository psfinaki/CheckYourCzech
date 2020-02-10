module AdjectiveComparative

open Microsoft.WindowsAzure.Storage.Table

type AdjectiveComparative(word: string,
                          positive: string,
                          comparatives: string,
                          isRegular: bool) =

    inherit TableEntity(word, word)
    
    new() = AdjectiveComparative(null, null, null, false)

    member val Positive = positive with get, set
    member val Comparatives = comparatives with get, set
    member val IsRegular = isRegular with get, set
