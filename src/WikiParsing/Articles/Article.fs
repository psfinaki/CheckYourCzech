module Article

open System
open FSharp.Data
open Html
open System.Collections.Generic

type Article = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let wikiUrl = "https://cs.wiktionary.org/wiki/"
let navigationId = "mw-navigation"
let lockInfoIndicator = "[e]"
let contentClass = "mw-parser-output"
let headerClass = "mw-headline"
let tableElementName = "table"
let tableNameElementName = "caption"

let getUrl = (+) wikiUrl

let getHtml = 
    Uri.EscapeDataString
    >> getUrl
    >> Article.Load
    >> fun data -> data.Html.Descendants()

let getContent =
    getHtml
    >> getNodeByClass contentClass
    >> fun node -> node.Elements()

let tryGetContent word =
    try 
        getContent word   
        |> Some 
    with | :? KeyNotFoundException | :? ArgumentException -> 
        None

let getTables nodes =
    let isTable     (node: HtmlNode) = node.HasName tableElementName
    let isTableName (node: HtmlNode) = node.HasName tableNameElementName

    let getTableName (table: HtmlNode) =
        table.Elements()
        |> Seq.filter isTableName
        |> Seq.exactlyOne
        |> fun node -> node.DirectInnerText()
        // for some reason, extracted captions end with an extra space
        |> fun name -> name.TrimEnd()

    let getTable table = (getTableName table, table)

    nodes
    |> Seq.filter isTable
    |> Seq.map getTable

let getHeaderName (header: HtmlNode) = 
    header.Elements()
    |> getNodeByClass headerClass
    |> fun node -> node.DirectInnerText()

let getParts name =
    Seq.skipWhile (not << isHeader)
    >> Seq.splitBy isHeader
    >> Seq.map Seq.behead
    >> Seq.map (fun (header, nodes) -> (getHeaderName header, nodes))
    >> Seq.where (fun (header, _) -> header = name)

let getChildrenParts elements =
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

let getChildrenPartsWhen filter =
    getChildrenParts
    >> Seq.where (fun (header, _) -> filter header)
    >> Seq.map snd

let getChildPart name =
    getChildrenParts
    >> Seq.where (fun (header, _) -> header = name)
    >> Seq.exactlyOne
    >> snd

let hasChildPart name elements = 
    try 
        getChildPart name elements |> ignore
        true
    with | :? KeyNotFoundException | :? ArgumentException ->
        false

let hasChildrenPartsWhen filter elements = 
    try 
        getChildrenPartsWhen filter elements
        |> (not << Seq.isEmpty)
    with | :? KeyNotFoundException | :? ArgumentException -> 
        false

let getInfo text nodes =
    nodes
    |> getNodeByInnerText text
    |> getInnerText

let hasInfo info elements = 
    try 
        getInfo info elements |> ignore
        true
    with | :? KeyNotFoundException | :? ArgumentException ->
        false

let isLocked word = 
    let getLockInfo = 
        getHtml
        >> getNodeById navigationId
        >> fun node -> node.GetInnermostAttributeWithText lockInfoIndicator
        >> fun attribute -> attribute.Value()

    match getLockInfo word with
    | s when s.Contains "Tato stránka je zamčena" -> true
    | s when s.Contains "Editovat tuto stránku" -> false
    | _ -> invalidArg word "odd article"

let isEditable = not << isLocked

let getPartsOfSpeech word =
    let partsOfSpeech = [ "podstatné jméno"; "přídavné jméno"; "sloveso" ]
    let isPartOfSpeech s = partsOfSpeech |> Seq.contains s

    let content = getContent word
    if content |> hasChildPart "čeština"
    then
        content
        |> getChildPart "čeština"
        |> getChildrenParts
        |> Seq.map fst
        |> Seq.filter isPartOfSpeech
    else
        Seq.empty

let getArticleName = 
    let isTitleTag (node: HtmlNode) = node.Name() = "title"
    let extractName (title: HtmlNode) = title.InnerText().Replace(" – Wikislovník", "")

    getHtml
    >> Seq.filter isTitleTag
    >> Seq.exactlyOne
    >> extractName
