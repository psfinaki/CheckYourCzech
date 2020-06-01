module Core.Verbs.ParticipleBuilder

open Common.StringHelper
open VerbPatternDetector
open Core.Reflexives

let buildParticipleTisknout = removeLast 4 >> append "l"
let buildParticipleMinout = removeLast 4 >> append "nul"
let buildParticipleCommon = removeLast 1 >> append "l"

let buildParticipleNonReflexive = function
    | verb when verb |> isPatternTisknout -> buildParticipleTisknout verb
    | verb when verb |> isPatternMinout -> buildParticipleMinout verb
    | verb -> buildParticipleCommon verb

let buildParticipleReflexive verb =
    let reflexive = getReflexive verb
    
    verb
    |> removeReflexive
    |> buildParticipleNonReflexive
    |> append (" " + reflexive)

let buildParticiple verb =
    if 
        verb |> isReflexive
    then 
        verb |> buildParticipleReflexive
    else 
        verb |> buildParticipleNonReflexive
