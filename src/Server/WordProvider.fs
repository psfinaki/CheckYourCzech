module WordProvider

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

let getDeclination (node: HtmlNode) =
    node
    |> getNodeChildren
    |> Seq.tryFind (getNodeName >> (=) "skloňování")

let isProperNoun =
    getContent
    >> Option.bind getCzechPart
    >> Option.bind getCzechNounPart
    >> Option.bind getDeclination
    >> Option.isSome

let hasGender gender =
    AnswerProvider.getGender
    >> (=) gender

let hasPlural = 
    AnswerProvider.getPlural 
    >> Array.isEmpty
    >> not

let isAppropriate gender word = 
    isProperNoun word
    && hasGender gender word
    && hasPlural word

let getRandomWord () =
    randomWikiArticleUrl
    |> WikiArticle.Load 
    |> fun node -> node.Html.Elements().[0].Elements().[0].Elements().[1].Elements().[0].InnerText().Split(" – ").[0]

let getNoun gender = 
    fun _ -> getRandomWord()
    |> Seq.initInfinite
    |> Seq.find (isAppropriate gender)