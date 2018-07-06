module Word

open System.Collections.Generic
open FSharp.Data

type WikiArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let wikiUrl = "https://cs.wiktionary.org/wiki/"
let randomWikiArticleUrl = "https://cs.wiktionary.org/wiki/Speciální:Náhodná_stránka"

let getNodeName (x: HtmlNode) = 
    x.Elements().[0].Attributes().[0].Value().Trim('#')

let getNodeChildren (x: HtmlNode) = 
    x.Elements().[1].Elements()

let getContent word =
    try
        wikiUrl + word
        |> WikiArticle.Load
        |> fun data -> data.Lists.Obsah.Html
        |> Some
    with
        | :? KeyNotFoundException ->
           None

let getCzechPart (html: HtmlNode) =
    html.Elements()
    |> Seq.tryFind (getNodeName >> (=) "čeština")

let getCzechNounPart (node: HtmlNode) =
    node
    |> getNodeChildren
    |> Seq.tryFind (getNodeName >> (=) "podstatné_jméno")

let getCzechAdjectivePart (node: HtmlNode) =
    node
    |> getNodeChildren
    |> Seq.tryFind (getNodeName >> (=) "přídavné_jméno")

let getDeclension (node: HtmlNode) =
    node
    |> getNodeChildren
    |> Seq.tryFind (getNodeName >> (=) "skloňování")

let getComparison (node: HtmlNode) =
    node
    |> getNodeChildren
    |> Seq.tryFind (getNodeName >> (=) "stupňování")

let isProperNoun =
    getContent
    >> Option.bind getCzechPart
    >> Option.bind getCzechNounPart
    >> Option.bind getDeclension
    >> Option.isSome

let hasGender gender =
    Noun.getGender
    >> (=) gender

let hasPlural = 
    Noun.getPlural 
    >> Array.isEmpty
    >> not

let isAppropriateNoun gender word = 
    isProperNoun word
    && hasGender gender word
    && hasPlural word

let isAdjective =
    getContent
    >> Option.bind getCzechPart
    >> Option.bind getCzechAdjectivePart
    >> Option.bind getComparison
    >> Option.isSome