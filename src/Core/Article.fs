module Article

open FSharp.Data

type Article = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let wikiUrl = "https://cs.wiktionary.org/wiki/"
let contentClass = "mw-parser-output"
let navigationId = "mw-head"
let headerClass  = "mw-headline"
let headerTags   = [ "h2"; "h3"; "h4"; "h5"; "h6" ]

let loadArticle word = Article.Load (wikiUrl + word)

let getNameFromHtml (html: HtmlDocument) = 
    let isTitleTag  (node: HtmlNode)  = node.Name() = "title"
    let extractName (title: HtmlNode) = title.InnerText().Split(" – ").[0]

    html.Descendants isTitleTag
    |> Seq.exactlyOne
    |> extractName

// serves only for testing getNameFromHtml
let getName word = 
    loadArticle word
    |> fun data -> data.Html
    |> getNameFromHtml

let getTableOfContents word =
    loadArticle word
    |> fun data -> data.Lists.Obsah.Html

let getContent word = 
    let getEverything (data: Article)  = data.Html.Body().Descendants()
    let isContentPart (node: HtmlNode) = node.HasClass contentClass

    let getContentPart = Seq.where isContentPart >> Seq.exactlyOne

    loadArticle word
    |> getEverything
    |> getContentPart
    |> fun node -> node.Elements()

let getInfo (name: string) elements =
    elements
    |> Seq.collect (fun (node: HtmlNode) -> node.Descendants())
    |> Seq.map     (fun (node: HtmlNode) -> node.DirectInnerText())
    |> Seq.where   (fun (text: string)   -> text.Contains name)
    |> Seq.distinct
    |> Seq.exactlyOne

let getEditInfo word = 
    let getEverything (data: Article)  = data.Html.Body().Descendants()
    let isNavigationPart (node: HtmlNode) = node.HasId navigationId

    let getNavigationPart = Seq.where isNavigationPart >> Seq.exactlyOne

    word
    |> loadArticle
    |> getEverything
    |> getNavigationPart
    |> fun node -> node.Descendants()
    |> Seq.collect (fun (node: HtmlNode) -> node.Attributes())
    |> Seq.map     (fun (attr: HtmlAttribute) -> attr.Value())
    |> Seq.where   (fun (text: string) -> text.Contains "[e]")
    |> Seq.exactlyOne

let isLocked word = 
    match (getEditInfo word) with
    | s when s.Contains "Tato stránka je zamčena" -> true
    | s when s.Contains "Editovat tuto stránku" -> false
    | _ -> invalidArg word "odd article"

let isHeader (node: HtmlNode) = 
    headerTags 
    |> Seq.contains (node.Name())

let getHeaderName (header: HtmlNode) = 
    header.Elements()
    |> Seq.filter (fun node -> node.HasClass headerClass)
    |> Seq.exactlyOne
    |> fun node -> node.DirectInnerText()

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

let tryFunc1 func x   = try func x   |> Some with _ -> None
let tryFunc2 func x y = try func x y |> Some with _ -> None

let tryGetTableOfContents word = tryFunc1 getTableOfContents word
let tryGetContent word         = tryFunc1 getContent word
let tryGetPart name elements   = tryFunc2 getPart name elements
let tryGetInfo name elements   = tryFunc2 getInfo name elements
