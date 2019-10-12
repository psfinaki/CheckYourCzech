module VerbParticiple

open Microsoft.WindowsAzure.Storage.Table
open Verb
open Storage        

type VerbParticiple(word) =
    inherit TableEntity(word, word)

    let getInfinitive = map id serializeObject ""
    let getParticiples = map getParticiples serializeObject ""
    let getPattern = map getParticiplePattern serializeString ""
    let getIsRegular = map hasRegularParticiple id false 
     
    new() =  VerbParticiple null

    member val Infinitive = getInfinitive word with get, set
    member val Participles = getParticiples word with get, set
    member val Pattern = getPattern word with get, set
    member val IsRegular = getIsRegular word with get, set
