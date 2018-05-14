module ServerCode.WordProvider

open System.Collections.Generic
open FSharp.Data

type WikiArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let wikiUrl = "https://cs.wiktionary.org/wiki/"
let randomWikiArticleUrl = "https://cs.wiktionary.org/wiki/Speciální:Náhodná_stránka"

let getListItem (x: HtmlNode) = 
    x.Elements().[0].Attributes().[0].Value().Trim('#')

let getLanguageParts word =
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
    |> Seq.tryFind (getListItem >> (=) "čeština")

let getCzechNounPart (html: HtmlNode) =
    html.Elements().[1].Elements()
    |> Seq.tryFind (getListItem >> (=) "podstatné_jméno")

let getDeclination (html: HtmlNode) =
    html.Elements().[1].Elements()
    |> Seq.tryFind (getListItem >> (=) "skloňování")

let isAppropriate word =
    word
    |> getLanguageParts
    |> Option.bind getCzechPart
    |> Option.bind getCzechNounPart
    |> Option.bind getDeclination
    |> Option.isSome

let getRandomWord () =
    randomWikiArticleUrl
    |> WikiArticle.Load 
    |> fun node -> node.Html.Elements().[0].Elements().[0].Elements().[1].Elements().[0].InnerText().Split(" – ").[0]

let getNoun () = 
    fun _ -> getRandomWord()
    |> Seq.initInfinite
    |> Seq.find isAppropriate