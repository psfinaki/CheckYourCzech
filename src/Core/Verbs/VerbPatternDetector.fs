module VerbPatternDetector

open StringHelper
open Letters
open Stem
open Reflexives
open VerbClasses
open VerbArticle

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
    | verb when verb |> ends "ést" -> Some "nést"
    | verb when verb |> ends "íst" -> Some "číst"
    | verb when verb |> ends "ct" -> Some "péct"
    | verb when verb |> ends "ít" -> Some "třít"
    | verb when verb |> ends "át" -> Some "brát"
    | verb when verb |> ends "at" -> Some "mazat"
    | _ -> None

let getPatternClassNE = function
    | verb when verb |> isPatternTisknout -> Some "tisknout"
    | verb when verb |> isPatternMinout -> Some "minout"
    | verb when verb |> ends "ít" -> Some "začít"
    | _ -> None

let getPatternClassJE = function
    | verb when verb |> ends "ovat" -> Some "kupovat"
    | verb when verb |> ends "ýt" -> Some "krýt"
    | _ -> None

let getPatternClassÍ = function
    | verb when verb |> isPatternProsit -> Some "prosit"
    | verb when verb |> isPatternČistit -> Some "čistit"
    | verb when verb |> ends "ět" -> Some "trpět"
    | verb when verb |> ends "et" -> Some "sázet"
    | _ -> None

let getPatternClassÁ = function
    | verb when verb |> ends "at" -> Some "dělat"
    | _ -> None

let patternClassMap =
    dict [ (E, getPatternClassE)
           (NE, getPatternClassNE)
           (JE, getPatternClassJE)
           (Í, getPatternClassÍ)
           (Á, getPatternClassÁ) ]

let getPatternByClass verb verbClass = patternClassMap.[verbClass] verb

let getPattern verb = 
    verb
    |> getThirdPersonSingular
    |> Seq.tryExactlyOne
    |> Option.map removeReflexive
    |> Option.map getClassByThirdPersonSingular
    |> Option.bind (getPatternByClass (verb |> removeReflexive))
