module VerbConjugation

open Microsoft.WindowsAzure.Storage.Table

type VerbConjugation(word: string,
                     infinitive: string,
                     pattern: string,
                     firstSingular: string,
                     secondSingular: string,
                     thirdSingular: string,
                     firstPlural: string,
                     secondPlural: string,
                     thirdPlural: string) =

    inherit TableEntity(word, word)
     
    new() =  VerbConjugation(null, null, null, null, null, null, null, null, null)

    member val Infinitive = infinitive with get, set
    member val Pattern = pattern with get, set
    
    member val FirstSingular = firstSingular with get, set
    member val SecondSingular = secondSingular with get, set
    member val ThirdSingular = thirdSingular with get, set
    member val FirstPlural = firstPlural with get, set
    member val SecondPlural = secondPlural with get, set
    member val ThirdPlural = thirdPlural with get, set
