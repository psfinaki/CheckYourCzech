﻿module Html

open FSharp.Data

type Article = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

type HtmlNode with
    member this.GetInnermostAttributeWithText text =
        this.Descendants()
        |> Seq.collect (fun node -> node.Attributes())
        |> Seq.map     (fun attr -> attr, attr.Value())
        |> Seq.where   (fun (attr, value) -> value.Contains text)
        |> Seq.exactlyOne
        |> fst

let headerTags   = [ "h1"; "h2"; "h3"; "h4"; "h5"; "h6" ]

let getNodeByInnerText text =
    Seq.collect  (fun (node: HtmlNode) -> node.Descendants())
    >> Seq.map   (fun node -> node, node.DirectInnerText())
    >> Seq.where (fun (node, innerText) -> innerText.Contains text)
    >> Seq.distinctBy snd
    >> Seq.exactlyOne
    >> fst

let getNodeByClass cssClass =
    Seq.where (fun (node: HtmlNode) -> node.HasClass cssClass)
    >> Seq.exactlyOne

let getNodeById id =
    Seq.where (fun (node: HtmlNode) -> node.HasId id)
    >> Seq.exactlyOne

let isHeader (node: HtmlNode) = 
    headerTags 
    |> Seq.contains (node.Name())
