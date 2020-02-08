module VerbImperative

open Microsoft.WindowsAzure.Storage.Table

type VerbImperative(word: string,
                    indicative: string,
                    imperatives: string,
                    ``class``: string,
                    pattern: string) =

    inherit TableEntity(word, word)

    new() = VerbImperative(null, null, null, null, null)

    member val Indicative = indicative with get, set
    member val Imperatives = imperatives with get, set
    member val Class = ``class`` with get, set
    member val Pattern = pattern with get, set
