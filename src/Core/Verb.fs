module Verb

open FSharp.Data
open StringHelper
open WikiString
open Article
open ArticleParser

type WikiVerb = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">
type WikiParticiplesTable2 = HtmlProvider<"https://cs.wiktionary.org/wiki/musit">
type WikiParticiplesTable3 = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">

let wikiUrl = "https://cs.wiktionary.org/wiki/"

let getVerbProvider =
    (+) wikiUrl
    >> WikiVerb.Load

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
    |> getPart "čeština"
    |> getPart "sloveso"
    |> getTables
    |> Seq.map fst
    |> Seq.findIndex ((=) "Příčestí")
    |> getParticipleByTableIndex word

let isReflexive verb = 
    verb |> ends " se" ||
    verb |> ends " si"

let removeReflexive = remove " se" >> remove " si"

let hasConjugation =
    tryGetVerb
    >> Option.bind (tryGetPart "časování")
    >> Option.isSome

let tryGetReflexive = function
    | word when word |> ends " se" -> Some "se"
    | word when word |> ends " si" -> Some "si"
    | _ -> None

let hasImperative = 
    tryGetVerb
    >> Option.bind (tryGetPart "časování")
    >> Option.map getTables
    >> Option.map (Seq.map fst)
    >> Option.map (Seq.contains "Rozkazovací způsob")
    >> Option.contains true

let getImperatives verb =
    let data = getVerbProvider verb
    let answer = data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - 2.``
    getForms answer

let getParticiples = getWikiParticiples >> getForms

let hasArchaicEnding verb = 
    verb |> ends "ti" || 
    verb |> ends "ci"

let isArchaic = removeReflexive >> hasArchaicEnding

let isModern = not << isArchaic

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
