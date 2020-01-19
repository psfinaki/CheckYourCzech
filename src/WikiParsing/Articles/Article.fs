module Article

open System
open FSharp.Data
open Html
open System.Collections.Generic
open System.Net.Http
open StringHelper

type Article = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let wikiUrl = "https://cs.wiktionary.org/wiki/"
let navigationId = "mw-navigation"
let lockInfoIndicator = "[e]"
let contentClass = "mw-parser-output"
let headerClass = "mw-headline"
let tableElementName = "table"
let tableNameElementName = "caption"

type MatchCondition =
    | Any
    | Is of string
    | Starts of string
    | OneOf of seq<string>

let getCondition = function
    | Any -> fun _ -> true
    | Is partName -> (=) partName
    | Starts partName -> starts partName
    | OneOf partNames -> fun x -> partNames |> Seq.contains x

let isReachable (url: string) = 
    (new HttpClient()).GetAsync(url) 
    |> Async.AwaitTask 
    |> Async.RunSynchronously
    |> fun response -> response.IsSuccessStatusCode

let getUrl = (+) wikiUrl

let getArticle entry = 
    let url = entry |> getUrl
    if url |> isReachable
    then 
        url
        |> Article.Load
        |> fun data -> data.Html.Descendants()
        |> Some
    else
        None

let getContent =
    getArticle
    >> Option.map (getNodeByClass contentClass)
    >> Option.map (fun node -> node.Elements())

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

let getPartsWhen filter =
    getParts
    >> Seq.where (fun (header, _) -> filter header)
    >> Seq.map snd

let getPart name =
    getParts
    >> Seq.where (fun (header, _) -> header = name)
    >> Seq.tryExactlyOne
    >> Option.map snd

let hasPartsWhen filter elements = 
    try 
        getPartsWhen filter elements
        |> (not << Seq.isEmpty)
    with | :? KeyNotFoundException | :? ArgumentException -> 
        false

let getInfos filter =
    getNodesByInnerText (getCondition filter)
    >> Seq.map getInnerText

let hasInfo info = 
    getInfos info
    >> (not << Seq.isEmpty)

let isLocked word = 
    let getLockIndicator (node: HtmlNode) =
        (node.GetInnermostAttributeWithText lockInfoIndicator).Value()

    let getLockInfo = 
        getArticle
        >> Option.map (getNodeById navigationId)
        >> Option.map getLockIndicator

    match getLockInfo word with
    | Some s when s.Contains "Tato stránka je zamčena" -> true
    | Some s when s.Contains "Editovat tuto stránku" -> false
    | _ -> invalidArg word "odd article"

let isEditable = not << isLocked

let getPartsOfSpeech =
    let partsOfSpeech = [ "podstatné jméno"; "přídavné jméno"; "sloveso" ]
    let isPartOfSpeech s = partsOfSpeech |> Seq.contains s

    getContent
    >> Option.bind (getPart "čeština")
    >> Option.map (getParts >> Seq.map fst >> Seq.filter isPartOfSpeech)
    >> Option.defaultValue Seq.empty

let rec private getPartMatch parts nodes = 
    if parts |> Seq.isEmpty
    then 
        Some nodes
    else
        let (head, tail) = parts |> Seq.behead
        let partCondition = getCondition head
        if nodes |> hasPartsWhen partCondition
        then 
            nodes
            |> getPartsWhen partCondition
            |> Seq.exactlyOne
            |> getPartMatch tail
        else 
            None

let rec private getPartMatches parts (nodes: seq<HtmlNode>) = 
    if parts |> Seq.isEmpty
    then 
        seq { nodes }
    else
        let (head, tail) = parts |> Seq.behead
        let partCondition = getCondition head
        if nodes |> hasPartsWhen partCondition
        then
            nodes
            |> getPartsWhen partCondition
            |> Seq.collect (getPartMatches tail)
        else 
            Seq.empty

let getCzechContent = 
    getContent
    >> Option.bind (getPart "čeština")

let ``match`` parts =
    getCzechContent
    >> Option.bind (getPartMatch parts)

let matches parts =
    getCzechContent
    >> Option.map (getPartMatches parts)
    >> Option.defaultValue Seq.empty

let isMatch parts = matches parts >> (not << Seq.isEmpty)
