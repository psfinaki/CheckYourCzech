module Core.Verbs.VerbPatternDetector

open Common.StringHelper
open Common.Verbs
open Common.Conjugation
open Core.Letters
open Core.Stem
open Core.Reflexives
open VerbClasses

let isPatternTisknoutNonReflexive word = 
    let getStem = removeLast 4
    
    let isPattern letter = 
        letter |> isConsonant && 
        letter |> isNotSyllabicConsonant
    
    word |> ends "nout" && 
    word |> getStem |> endsIf isPattern

let isPatternMinoutNonReflexive word = 
    let getStem = removeLast 4
    
    let isPattern letter =
        letter |> isVowel ||
        letter |> isSyllabicConsonant

    word |> ends "nout" &&
    word |> getStem |> endsIf isPattern

let isPatternPrositNonReflexive word = 
    word |> ends "it" &&
    word |> removeLast 2 |> takeLast 2 |> (not << isConsonantGroup)

let isPatternČistitNonReflexive word =
    word |> ends "it" &&
    word |> removeLast 2 |> takeLast 2 |> isConsonantGroup

let isPatternTisknout = removeReflexive >> isPatternTisknoutNonReflexive
let isPatternMinout = removeReflexive >> isPatternMinoutNonReflexive
let isPatternProsit = removeReflexive >> isPatternPrositNonReflexive
let isPatternČistit = removeReflexive >> isPatternČistitNonReflexive

let invalidVerb verb verbClass = 
    sprintf "The verb %s does not belong to the class %i." verb verbClass
    |> invalidArg "verb"

let getPatternClassE = function
    | verb when verb |> ends "ést" -> Some Nést
    | verb when verb |> ends "íst" -> Some Číst
    | verb when verb |> ends "ct" -> Some Péct
    | verb when verb |> ends "ít" -> Some Třít
    | verb when verb |> ends "át" -> Some Brát
    | verb when verb |> ends "at" -> Some Mazat
    | _ -> None

let getPatternClassNE = function
    | verb when verb |> isPatternTisknout -> Some Tisknout
    | verb when verb |> isPatternMinout -> Some Minout
    | verb when verb |> ends "ít" -> Some Začít
    | _ -> None

let getPatternClassJE = function
    | verb when verb |> ends "ovat" -> Some Kupovat
    | verb when verb |> ends "ýt" -> Some Krýt
    | _ -> None

let getPatternClassÍ = function
    | verb when verb |> isPatternProsit -> Some Prosit
    | verb when verb |> isPatternČistit -> Some Čistit
    | verb when verb |> ends "ět" -> Some Trpět
    | verb when verb |> ends "et" -> Some Sázet
    | _ -> None

let getPatternClassÁ = function
    | verb when verb |> ends "at" -> Some Dělat
    | _ -> None

let getPatternByClass verb = function
    | VerbClass.E ->
        verb
        |> getPatternClassE
        |> Option.map ConjugationPattern.ClassE
    | VerbClass.NE ->
        verb
        |> getPatternClassNE
        |> Option.map ConjugationPattern.ClassNE
    | VerbClass.JE ->
        verb
        |> getPatternClassJE
        |> Option.map ConjugationPattern.ClassJE
    | VerbClass.Í ->
        verb
        |> getPatternClassÍ
        |> Option.map ConjugationPattern.ClassÍ
    | VerbClass.Á ->
        verb
        |> getPatternClassÁ
        |> Option.map ConjugationPattern.ClassÁ

let getPattern verb = 
    removeReflexive
    >> getClassByThirdPersonSingular
    >> getPatternByClass (verb |> removeReflexive)
