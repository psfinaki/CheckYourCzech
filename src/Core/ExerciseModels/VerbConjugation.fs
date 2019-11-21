module VerbConjugation

open Microsoft.WindowsAzure.Storage.Table
open Verb
open Storage        

type VerbConjugation(word) =
    inherit TableEntity(word, word)

    let getInfinitive = map id serializeObject ""
    let getConjugations = map getConjugations serializeObject ""
    let getPattern = map getParticiplePattern serializeString ""
     
    new() =  VerbConjugation null

    member val Infinitive = getInfinitive word with get, set
    member val Conjugations = getConjugations word with get, set
    member val Pattern = getPattern word with get, set
