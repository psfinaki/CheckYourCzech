module Word

open System.Collections.Generic
open FSharp.Data

type WikiArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let wikiUrl = "https://cs.wiktionary.org/wiki/"

let getContent word =
    try
        wikiUrl + word
        |> WikiArticle.Load
        |> fun data -> data.Lists.Obsah.Html
        |> Some
    with
        | :? KeyNotFoundException ->
           None

let getNodeName (x: HtmlNode) = 
    x.Elements().[0].Attributes().[0].Value().Trim('#')

let getPart nodeName (x: HtmlNode) =
    x.Elements().[1].Elements()
    |> Seq.tryFind (getNodeName >> (=) nodeName)

let getCzechPart (html: HtmlNode) =
    html.Elements()
    |> Seq.tryFind (getNodeName >> (=) "čeština")

let isNoun =
    getContent
    >> Option.bind getCzechPart
    >> Option.bind (getPart "podstatné_jméno")
    >> Option.bind (getPart "skloňování")
    >> Option.isSome

let isAdjective =
    getContent
    >> Option.bind getCzechPart
    >> Option.bind (getPart "přídavné_jméno")
    >> Option.bind (getPart "stupňování")
    >> Option.isSome
