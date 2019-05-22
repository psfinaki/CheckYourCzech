module Adjective

open FSharp.Data
open Article
open WikiString
open StringHelper

type WikiAdjective = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">
type WikiAdjectiveNový = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">
type WikiAdjectiveStarý = HtmlProvider<"https://cs.wiktionary.org/wiki/starý">
type WikiAdjectiveVrchní = HtmlProvider<"https://cs.wiktionary.org/wiki/vrchní">

let getAdjectiveProvider =
    (+) wikiUrl
    >> WikiAdjective.Load

let hasDeclension = 
    ArticleParser.tryGetAdjective
    >> Option.bind (tryGetChildPart "skloňování")
    >> Option.isSome

let hasComparison = 
    ArticleParser.tryGetAdjective
    >> Option.bind (tryGetChildPart "stupňování")
    >> Option.isSome

let isSyntacticComparison (comparison: string) = comparison.StartsWith "více "

let isPossessive adjective = 
    adjective |> ends "ův" ||
    adjective |> ends "in"

let isNominalized = 
    ArticleParser.tryGetNoun
    >> Option.bind (tryGetChildPart "skloňování")
    >> Option.isSome

let getPluralDobrý = 
    (+) wikiUrl
    >> WikiAdjectiveNový.Load
    >> fun data -> data.Tables.``Skloňování[editovat]``.Rows.[0].``plurál - mužský životný``

let getPluralStarý = 
    (+) wikiUrl
    >> WikiAdjectiveStarý.Load
    >> fun data -> data.Tables.``Skloňování[editovat]2``.Rows.[0].``plurál - mužský životný``

let getPluralVrchní = 
    (+) wikiUrl
    >> WikiAdjectiveVrchní.Load
    >> fun data -> data.Tables.``Skloňování[editovat]3``.Rows.[0].``plurál - mužský životný``

let pluralDeclensionsMap =
    dict [ (1, getPluralDobrý)
           (2, getPluralStarý)
           (3, getPluralVrchní) ]

let countDeclensions = 
    Article.getContent
    >> Article.getChildPart "čeština"
    >> Article.getParts "skloňování"
    >> Seq.length

let getPlural adjective = 
    adjective
    |> countDeclensions
    |> fun n -> pluralDeclensionsMap.[n] adjective

let getPositive =
    getAdjectiveProvider
    >> fun data -> data.Tables.``Stupňování[editovat]``.Rows.[0].tvar

let getComparatives =
    getAdjectiveProvider
    >> fun data -> data.Tables.``Stupňování[editovat]``.Rows.[1].tvar
    >> getForms

let isHardPositive = endsOneOf ["ý"; "á"; "é"]

let isSoftPositive adjective = 
    adjective |> ends "í" && 
    adjective |> (not << ends "ší")

let isPositive adjective = 
    adjective |> isHardPositive || 
    adjective |> isSoftPositive

let hasMorphologicalComparatives = 
    let isMorphological = not << isSyntacticComparison
    
    getComparatives 
    >> Array.filter isMorphological
    >> Seq.isEmpty 
    >> not
