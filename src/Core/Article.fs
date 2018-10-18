module Article

open FSharp.Data
open Html

type Article = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let wikiUrl = "https://cs.wiktionary.org/wiki/"
let contentClass = "mw-parser-output"
let headerClass  = "mw-headline"
let navigationId = "mw-navigation"
let lockInfoIndicator = "[e]"

let loadArticle word = Article.Load (wikiUrl + word)

let getEverything (data: Article)  = data.Html.Body().Descendants()

let getNameFromHtml (html: HtmlDocument) = 
    let isTitleTag  (node: HtmlNode)  = node.Name() = "title"
    let extractName (title: HtmlNode) = title.InnerText().Split(" – ").[0]

    html.Descendants isTitleTag
    |> Seq.exactlyOne
    |> extractName

let getHeaderName (header: HtmlNode) = 
    header.Elements()
    |> getNodeByClass headerClass
    |> fun node -> node.DirectInnerText()

let getLockInfo = 
    loadArticle
    >> getEverything
    >> getNodeById navigationId
    >> fun node -> node.GetInnermostAttributeWithText lockInfoIndicator
    >> fun attribute -> attribute.Value()

// serves only for testing getNameFromHtml
let getName = 
    loadArticle
    >> fun data -> data.Html
    >> getNameFromHtml

let getTableOfContents =
    loadArticle
    >> fun data -> data.Lists.Obsah.Html

let getContent =
    loadArticle
    >> getEverything
    >> getNodeByClass contentClass
    >> fun node -> node.Elements()

let getInfo text nodes =
    nodes
    |> getNodeByInnerText text
    |> fun (node: HtmlNode) -> node.DirectInnerText()

let getParts elements =
    let biggestHeader = 
        elements
        |> Seq.filter isHeader
        |> Seq.map (fun node -> node.Name())
        |> Seq.distinct
        |> Seq.sort
        |> Seq.tryHead

    match biggestHeader with
    | Some header ->
        let isBiggestHeader (node: HtmlNode) = node.Name() = header

        elements
        |> Seq.splitBy isBiggestHeader
        |> Seq.where (fun group -> group |> Seq.head |> isBiggestHeader)
        |> Seq.map Seq.behead
        |> Seq.map (fun (header, nodes) -> (getHeaderName header, nodes))
    | None -> 
        Seq.empty

let getPart name =
    getParts
    >> Seq.where (fun (header, _) -> header = name)
    >> Seq.exactlyOne
    >> snd

let isLocked word = 
    match getLockInfo word with
    | s when s.Contains "Tato stránka je zamčena" -> true
    | s when s.Contains "Editovat tuto stránku" -> false
    | _ -> invalidArg word "odd article"

let tryFunc1 func x   = try func x   |> Some with _ -> None
let tryFunc2 func x y = try func x y |> Some with _ -> None

let tryGetTableOfContents word = tryFunc1 getTableOfContents word
let tryGetContent word         = tryFunc1 getContent word
let tryGetPart name elements   = tryFunc2 getPart name elements
let tryGetInfo name elements   = tryFunc2 getInfo name elements
