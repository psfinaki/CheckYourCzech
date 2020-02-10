module NounAccusative

open Microsoft.WindowsAzure.Storage.Table

type NounAccusative(word: string,
                    nominative: string,
                    accusatives: string,
                    gender: string,
                    patterns: string) =

    inherit TableEntity(word, word)

    new() = NounAccusative(null, null, null, null, null)

    member val Nominative = nominative with get, set
    member val Accusatives = accusatives with get, set
    member val Gender = gender with get, set
    member val Patterns = patterns with get, set
