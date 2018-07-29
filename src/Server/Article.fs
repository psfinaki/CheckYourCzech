module Article

open FSharp.Data

type Article = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let wikiUrl = "https://cs.wiktionary.org/wiki/"
let contentClass = "mw-parser-output"
let headerClass  = "mw-headline"
let headerTags   = [ "h2"; "h3"; "h4"; "h5"; "h6" ]

let loadArticle word = Article.Load (wikiUrl + word)

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

// I failed to make this code expressive enough, thus comment.
// We need to go through elements, 
// find a header with required name,
// take into account its size,
// and stop iterating when meeting another header of this size.
let getPart name elements =
    let isHeaderTag   (node: HtmlNode) = headerTags |> Seq.contains (node.Name())
    let isHeaderClass (node: HtmlNode) = node.HasClass headerClass

    let getHeaderName (node: HtmlNode) = 
        node.Elements()
        |> Seq.where isHeaderClass
        |> Seq.exactlyOne
        |> fun node -> node.DirectInnerText()

    let size = 
        elements
        |> Seq.where isHeaderTag
        |> Seq.where (getHeaderName >> (=) name)
        |> Seq.exactlyOne
        |> fun node -> node.Name()

    let headerHasSize size (header: HtmlNode) = header.Name() = size
    let headerHasName name (header: HtmlNode) = getHeaderName header = name

    elements
    |> Seq.skipWhile (not << (fun node -> isHeaderTag node && node |> headerHasName name))
    |> Seq.skip 1
    |> Seq.takeWhile (not << (fun node -> isHeaderTag node && node |> headerHasSize size))

let getInfo name elements =
    elements
    |> Seq.collect (fun (node: HtmlNode) -> node.Descendants())
    |> Seq.map     (fun (node: HtmlNode) -> node.DirectInnerText())
    |> Seq.where   (fun (text: string)   -> text.Contains name)
    |> Seq.distinct
    |> Seq.exactlyOne

let tryFunc1 func x   = try func x   |> Some with _ -> None
let tryFunc2 func x y = try func x y |> Some with _ -> None

let tryGetTableOfContents word = tryFunc1 getTableOfContents word
let tryGetContent word         = tryFunc1 getContent word
let tryGetPart name elements   = tryFunc2 getPart name elements
let tryGetInfo name elements   = tryFunc2 getInfo name elements
