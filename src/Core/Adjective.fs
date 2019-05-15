module Adjective

open FSharp.Data
open Article
open WikiString

type WikiAdjective = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">
type WikiAdjectiveNominalized = HtmlProvider<"https://cs.wiktionary.org/wiki/starý">
type WikiAdjectiveNotNominalized = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">

let getAdjectiveProvider =
    (+) wikiUrl
    >> WikiAdjective.Load

let hasDeclension = 
    ArticleParser.tryGetAdjective
    >> Option.bind (tryGetPart "skloňování")
    >> Option.isSome

let hasComparison = 
    ArticleParser.tryGetAdjective
    >> Option.bind (tryGetPart "stupňování")
    >> Option.isSome

let isSyntacticComparison (comparison: string) = comparison.StartsWith "více "

let isNominalized = 
    ArticleParser.tryGetNoun
    >> Option.bind (tryGetPart "skloňování")
    >> Option.isSome

let getPlural = function
    | adjective when adjective |> isNominalized ->
        adjective
        |> (+) wikiUrl
        |> WikiAdjectiveNominalized.Load
        |> fun data -> data.Tables.``Skloňování[editovat]2``.Rows.[0].``plurál - mužský životný``
    | adjective when adjective |> (not << isNominalized) ->
        adjective
        |> (+) wikiUrl
        |> WikiAdjectiveNotNominalized.Load
        |> fun data -> data.Tables.``Skloňování[editovat]``.Rows.[0].``plurál - mužský životný``
    | adjective ->
        invalidOp ("Odd word: " + adjective)

let getPositive =
    getAdjectiveProvider
    >> fun data -> data.Tables.``Stupňování[editovat]``.Rows.[0].tvar

let getComparatives =
    getAdjectiveProvider
    >> fun data -> data.Tables.``Stupňování[editovat]``.Rows.[1].tvar
    >> getForms

let isPositive adjective = (adjective = getPositive adjective)

let hasMorphologicalComparatives = 
    let isMorphological = not << isSyntacticComparison
    
    getComparatives 
    >> Array.filter isMorphological
    >> Seq.isEmpty 
    >> not
