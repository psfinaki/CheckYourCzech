module VerbPatternDetector

open StringHelper
open Letters
open Stem

let isPatternTisknout word = 
    let getStem = removeLast 4
    
    let isPattern letter = 
        letter |> isConsonant && 
        letter |> isNotSyllabicConsonant
    
    word |> ends "nout" && 
    word |> getStem |> endsIf isPattern

let isPatternMinout word = 
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

let getPatternClass1 = function
    | verb when verb |> ends "ést" -> "nést"
    | verb when verb |> ends "íst" -> "číst"
    | verb when verb |> ends "ct" -> "péct"
    | verb when verb |> ends "ít" -> "třít"
    | verb when verb |> ends "át" -> "brát"
    | verb when verb |> ends "at" -> "mazat"
    | verb -> invalidVerb verb 1

let getPatternClass2 = function
    | verb when verb |> isPatternTisknout -> "tisknout"
    | verb when verb |> isPatternMinout -> "minout"
    | verb when verb |> ends "ít" -> "začít"
    | verb -> invalidVerb verb 2

let getPatternClass3 = function
    | verb when verb |> ends "ovat" -> "kupovat"
    | verb when verb |> ends "ýt" -> "krýt"
    | verb -> invalidVerb verb 3

let getPatternClass4 = function
    | verb when verb |> isPatternProsit -> "prosit"
    | verb when verb |> isPatternČistit -> "čistit"
    | verb when verb |> ends "ět" -> "trpět"
    | verb when verb |> ends "et" -> "sázet"
    | verb -> invalidVerb verb 4

let getPatternClass5 = function
    | verb when verb |> ends "at" -> "dělat"
    | verb -> invalidVerb verb 5

let patternClassMap =
    dict [ (1, getPatternClass1)
           (2, getPatternClass2)
           (3, getPatternClass3)
           (4, getPatternClass4)
           (5, getPatternClass5) ]

let getPatternByClass verb verbClass = 
    verb 
    |> Verb.removeReflexive 
    |> patternClassMap.[verbClass]
