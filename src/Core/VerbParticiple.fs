module VerbParticiple

open Microsoft.WindowsAzure.Storage.Table
        
let isRegular verb = 
    let theoretical = ParticipleBuilder.buildParticiple verb
    let practical = Verb.getParticiples verb
    practical |> Array.contains theoretical

let isValid word = 
    word |> Word.isVerb &&
    word |> Verb.hasConjugation &&
    word |> Verb.isModern

let getInfinitive = Storage.mapSafeString id 
let getParticiples = Storage.mapSafeString Verb.getParticiples
let getPattern = Storage.mapSafeObject (ParticiplePatternDetector.getPattern >> box)
let getRegularity = Storage.mapSafeBool isRegular 

type VerbParticiple(word) =
    inherit TableEntity(word, word)

    new() =  VerbParticiple null

    member val Infinitive = getInfinitive word with get, set
    member val Participles = getParticiples word with get, set
    member val Pattern = getPattern word with get, set
    member val IsRegular = getRegularity word with get, set

let record word =
    if 
        isValid word
    then 
        word |> VerbParticiple |> Storage.upsert "verbparticiples"
