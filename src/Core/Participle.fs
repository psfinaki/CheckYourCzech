module Participle

open Microsoft.WindowsAzure.Storage.Table
open StringHelper
open Stem
open VerbPatternDetector

type Pattern = 
    | Minout
    | Tisknout
    | Common

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

let getPattern = function
    | verb when verb |> isPatternMinout -> Minout
    | verb when verb |> isPatternTisknout -> Tisknout
    | _ -> Common

let isValid word = 
    word |> Word.isVerb &&
    word |> Verb.hasConjugation &&
    word |> Verb.isModern

type Participle(word) =
    inherit TableEntity(word, word)

    new() =  Participle null

    member val Infinitive  = word |> Storage.mapSafeString id                  with get, set
    member val Participles = word |> Storage.mapSafeString Verb.getParticiples      with get, set
    member val Pattern     = word |> Storage.mapSafeObject (getPattern >> box) with get, set
    member val IsRegular   = word |> Storage.mapSafeBool   isRegular           with get, set

let record word =
    if 
        isValid word
    then 
        word |> Participle |> Storage.upsert "participles"
