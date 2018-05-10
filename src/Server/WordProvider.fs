module ServerCode.WordProvider

open FSharp.Data

type WikiArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let wikiUrl = "https://cs.wiktionary.org/wiki/"
let randomWikiArticleUrl = "https://cs.wiktionary.org/wiki/Speciální:Náhodná_stránka"

let getListItem (x: HtmlNode) = 
    x.Elements().[0].Attributes().[0].Value().Trim('#')

let getLanguageParts word = 
    wikiUrl + word
    |> WikiArticle.Load
    |> fun data -> data.Lists.Obsah.Html.Elements()

let getCzechPart word =
    word
    |> getLanguageParts
    |> List.tryFind (getListItem >> (=) "čeština")

let isCzechNoun word =
    match getCzechPart word with
    | Some czechPart ->
        czechPart
        |> fun node -> node.Elements().[1].Elements()
        |> Seq.map getListItem
        |> Seq.exists ((=) "podstatné_jméno")
    | None ->
        false

let getRandomWord () =
    randomWikiArticleUrl
    |> WikiArticle.Load 
    |> fun node -> node.Html.Elements().[0].Elements().[0].Elements().[1].Elements().[0].InnerText().Split(" – ").[0]

let getCzechNoun () = 
    fun _ -> getRandomWord()
    |> Seq.initInfinite
    |> Seq.find isCzechNoun