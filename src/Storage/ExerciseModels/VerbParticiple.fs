module VerbParticiple

open Microsoft.WindowsAzure.Storage.Table

type VerbParticiple(word: string,
                    infinitive: string,
                    participles: string,
                    pattern: string,
                    isRegular: bool) =

    inherit TableEntity(word, word)

    new() =  VerbParticiple(null, null, null, null, false)

    member val Infinitive = infinitive with get, set
    member val Participles = participles with get, set
    member val Pattern = pattern with get, set
    member val IsRegular = isRegular with get, set
