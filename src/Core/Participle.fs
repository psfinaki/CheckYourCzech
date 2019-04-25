module Participle

open Microsoft.WindowsAzure.Storage.Table
open StringHelper
open Stem
open VerbPatternDetector

let hasStemPattern getStem isPattern = getStem >> endsIf isPattern

let buildParticipleTisknout = removeLast 4 >> append "l"
let buildParticipleMinout   = removeLast 4 >> append "nul"
let buildParticipleCommon   = removeLast 1 >> append "l"

let getReflexive = function
    | word when word |> ends " se" -> Some "se"
    | word when word |> ends " si" -> Some "si"
    | _ -> None

let buildTheoreticalParticiple verb =
    let buildTheoreticalParticipleNonReflexive = function
        | verb when verb |> isPatternTisknout -> buildParticipleTisknout verb
        | verb when verb |> isPatternMinout -> buildParticipleMinout verb
        | verb -> buildParticipleCommon verb

    let reflexive = getReflexive verb
    match reflexive with 
    | Some pronoun ->
        verb
        |> Verb.removeReflexive
        |> buildTheoreticalParticipleNonReflexive
        |> append (" " + pronoun)
    | None ->
        verb
        |> buildTheoreticalParticipleNonReflexive
        
let isRegular word = 
    let theoretical = buildTheoreticalParticiple word
    let practical = Verb.getParticiples word
    practical |> Array.contains theoretical

let isValid word = 
    word |> Word.isVerb &&
    word |> Verb.hasConjugation &&
    word |> Verb.isModern

let getInfinitive = Storage.mapSafeString id 
let getParticiples = Storage.mapSafeString Verb.getParticiples
let getPattern = Storage.mapSafeObject (ParticiplePatternDetector.getPattern >> box)
let getRegularity = Storage.mapSafeBool isRegular 

type Participle(word) =
    inherit TableEntity(word, word)

    new() =  Participle null

    member val Infinitive = getInfinitive word with get, set
    member val Participles = getParticiples word with get, set
    member val Pattern = getPattern word with get, set
    member val IsRegular = getRegularity word with get, set

let record word =
    if 
        isValid word
    then 
        word |> Participle |> Storage.upsert "participles"
