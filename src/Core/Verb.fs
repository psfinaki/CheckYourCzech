module Verb

open FSharp.Data
open StringHelper
open WikiString

type WikiVerb = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">

let wikiUrl = "https://cs.wiktionary.org/wiki/"

let getVerbProvider =
    (+) wikiUrl
    >> WikiVerb.Load

let removeReflexive = remove " se" >> remove " si"

let hasArchaicEnding verb = 
    verb |> ends "ti" || 
    verb |> ends "ci"

let isArchaic = removeReflexive >> hasArchaicEnding

let getThirdPersonSingular verb = 
    let data = getVerbProvider verb
    let answer = data.Tables.``Časování[editovat]``.Rows.[0].``Číslo jednotné - 3.``
    getForms answer

let getClassByThirdPersonSingular = function
    | form when form |> ends "á"  -> 5
    | form when form |> ends "í"  -> 4
    | form when form |> ends "je" -> 3
    | form when form |> ends "ne" -> 2
    | form when form |> ends "e"  -> 1
    | _ -> invalidArg "verb" "Incorrect third person singular."

let getClass =
    getThirdPersonSingular
    >> Seq.tryExactlyOne
    >> Option.map removeReflexive
    >> Option.map getClassByThirdPersonSingular
