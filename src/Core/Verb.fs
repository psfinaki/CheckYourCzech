module Verb

open StringHelper
open Letters
open Stem

let removeReflexive = remove " se" >> remove " si"

let hasArchaicEnding verb = 
    verb |> ends "ti" || 
    verb |> ends "ci"

let isArchaic = removeReflexive >> hasArchaicEnding

let getClassByThirdPersonSingular = function
    | form when form |> ends "á"  -> 5
    | form when form |> ends "í"  -> 4
    | form when form |> ends "je" -> 3
    | form when form |> ends "ne" -> 2
    | form when form |> ends "e"  -> 1
    | _ -> invalidArg "verb" "Incorrect third person singular."

let isPatternTisknout (word: string) = 
    let getStem = removeLast 4
    
    let isPattern letter = 
        letter |> isConsonant && 
        letter |> isNotSyllabicConsonant
    
    word |> ends "nout" && 
    word |> getStem |> endsIf isPattern

let isPatternMinout (word: string) = 
    let getStem = removeLast 4
    
    let isPattern letter =
        letter |> isVowel ||
        letter |> isSyllabicConsonant

    word |> ends "nout" &&
    word |> getStem |> endsIf isPattern

let isPatternProsit word = 
    word |> ends "it" &&
    word |> removeLast 2 |> takeLast 2 |> (not << isConsonantGroup)

let isPatternČistit word =
    word |> ends "it" &&
    word |> removeLast 2 |> takeLast 2 |> isConsonantGroup

let invalidVerb verb verbClass = 
    sprintf "The verb %s does not belong to the class %i." verb verbClass
    |> invalidArg "verb"

let getTemplateClass1 = function
    | verb when verb |> ends "ést" -> "nést"
    | verb when verb |> ends "íst" -> "číst"
    | verb when verb |> ends "éct" -> "péct"
    | verb when verb |> ends "ít" -> "třít"
    | verb when verb |> ends "át" -> "brát"
    | verb when verb |> ends "at" -> "mazat"
    | verb -> invalidVerb verb 1

let getTemplateClass2 = function
    | verb when verb |> isPatternTisknout -> "tisknout"
    | verb when verb |> isPatternMinout -> "minout"
    | verb when verb |> ends "ít" -> "začít"
    | verb -> invalidVerb verb 2

let getTemplateClass3 = function
    | verb when verb |> ends "ovat" -> "kupovat"
    | verb when verb |> ends "ýt" -> "krýt"
    | verb -> invalidVerb verb 3

let getTemplateClass4 = function
    | verb when verb |> isPatternProsit -> "prosit"
    | verb when verb |> isPatternČistit -> "čistit"
    | verb when verb |> ends "ět" -> "trpět"
    | verb when verb |> ends "et" -> "sázet"
    | verb -> invalidVerb verb 4

let getTemplateClass5 = function
    | verb when verb |> ends "at" -> "dělat"
    | verb -> invalidVerb verb 5

let templateClassMap =
    dict [ (1, getTemplateClass1)
           (2, getTemplateClass2)
           (3, getTemplateClass3)
           (4, getTemplateClass4)
           (5, getTemplateClass5) ]

let getTemplateByClass verb verbClass = 
    verb 
    |> removeReflexive 
    |> templateClassMap.[verbClass]
