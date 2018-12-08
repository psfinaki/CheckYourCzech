module Participle

open FSharp.Data
open Microsoft.WindowsAzure.Storage.Table
open Article
open WikiString
open StringHelper
open Letters
open Stem

type WikiParticiplesTable2 = HtmlProvider<"https://cs.wiktionary.org/wiki/musit">
type WikiParticiplesTable3 = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">

let hasStemPattern getStem isPattern = getStem >> endsIf isPattern

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

let buildParticipleTisknout = removeLast 4 >> append "l"
let buildParticipleMinout   = removeLast 4 >> append "nul"
let buildParticipleCommon   = removeLast 1 >> append "l"

let buildTheoreticalParticiple = function
    | verb when verb |> isPatternTisknout -> buildParticipleTisknout verb
    | verb when verb |> isPatternMinout -> buildParticipleMinout verb
    | verb -> buildParticipleCommon verb

let getParticiplesTable2 = 
    (+) "https://cs.wiktionary.org/wiki/"
    >> WikiParticiplesTable2.Load
    >> fun data -> data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - mužský životný i neživotný``

let getParticiplesTable3 =
    (+) "https://cs.wiktionary.org/wiki/"
    >> WikiParticiplesTable3.Load
    >> fun data -> data.Tables.``Časování[editovat]3``.Rows.[0].``Číslo jednotné - mužský životný i neživotný``

let getParticipleByTableIndex word n = 
    let participleExtraction = dict[ 
        1, getParticiplesTable2
        2, getParticiplesTable3 ]

    participleExtraction.Item n word

let getWikiParticiples word =
    word
    |> getContent
    |> getTables
    |> Seq.map fst
    |> Seq.findIndex ((=) "Příčestí")
    |> getParticipleByTableIndex word
        
let getParticiples = getWikiParticiples >> getForms

let isRegular word = 
    let theoretical = buildTheoreticalParticiple word
    let practical = getParticiples word
    practical |> Array.contains theoretical

let isValid =
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "sloveso")
    >> Option.bind (tryGetPart "časování")
    >> Option.isSome

type Participle(word) =
    inherit TableEntity(word, word)

    new() =  Participle null

    member val Infinitive  = word |> Storage.mapSafeString id             with get, set
    member val Participles = word |> Storage.mapSafeString getParticiples with get, set
    member val IsRegular   = word |> Storage.mapSafeBool   isRegular      with get, set

let record word =
    if 
        isValid word
    then 
        word |> Participle |> Storage.upsert "participles"