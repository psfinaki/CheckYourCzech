﻿module Word

open Article
open Storage

let recordCzechPartOfSpeech word = function
    | "podstatné jméno" -> [
        if word |> NounValidation.isPluralValid
        then word |> NounPlural.NounPlural |> upsert "nounplurals"

        if word |> NounValidation.isAccusativeValid
        then word |> NounAccusative.NounAccusative |> upsert "nounaccusatives"
      ]

    | "přídavné jméno" -> [
        if word |> AdjectiveValidation.isPluralValid
        then word |> AdjectivePlural.AdjectivePlural |> upsert "adjectiveplurals"

        if word |> AdjectiveValidation.isComparativeValid
        then word |> AdjectiveComparative.AdjectiveComparative |> upsert "adjectivecomparatives"
      ]
            
    | "sloveso" -> [
        if word |> VerbValidation.isImperativeValid
        then word |> VerbImperative.VerbImperative |> upsert "verbimperatives"

        if word |> VerbValidation.isParticipleValid
        then word |> VerbParticiple.VerbParticiple |> upsert "verbparticiples"

        if word |> VerbValidation.isConjugationValid
        then word |> VerbConjugation.VerbConjugation |> upsert "verbconjugation"
      ]

    | _ -> []
    
let record word =
    word
    |> getPartsOfSpeech
    |> Seq.collect (recordCzechPartOfSpeech word)
    |> Async.Parallel
    |> Async.Ignore
    |> Async.RunSynchronously

