module VerbConjugation

open Microsoft.WindowsAzure.Storage.Table
open Verb
open Storage    
open Conjugation    

type VerbConjugation(word) =
    inherit TableEntity(word, word)

    let getInfinitive = map id serializeObject ""
    let getConjugation p = map (getConjugation p) serializeObject ""
    let getPattern = map getParticiplePattern serializeString ""
     
    new() =  VerbConjugation null

    member val Infinitive = getInfinitive word with get, set
    member val Pattern = getPattern word with get, set

    member val FirstSingular = getConjugation FirstSingular word with get, set
    member val SecondSingular = getConjugation SecondSingular word with get, set
    member val ThirdSingular = getConjugation ThirdSingular word with get, set
    member val FirstPlural = getConjugation FirstPlural word with get, set
    member val SecondPlural = getConjugation SecondPlural word with get, set
    member val ThirdPlural = getConjugation ThirdPlural word with get, set
    
    member this.Conjugation = function
        | FirstSingular  -> this.FirstSingular
        | SecondSingular -> this.SecondSingular
        | ThirdSingular  -> this.ThirdSingular
        | FirstPlural    -> this.FirstPlural
        | SecondPlural   -> this.SecondPlural
        | ThirdPlural    -> this.ThirdPlural