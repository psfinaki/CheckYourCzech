module Noun

open FSharp.Data
open Gender

type Wiki = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let getPlural word = 
    let url = "https://cs.wiktionary.org/wiki/" + word
    let data = Wiki.Load url
    let answer = data.Tables.``Skloňování[editovat]``.Rows.[0].plurál
    
    match answer with
    | "—" | "" -> [||]
    | _ -> answer.Split "/" |> Array.map (fun s -> s.Trim()) 

let hasPlural = getPlural >> (not << Array.isEmpty)

let getGender word =
    let getDirectInnerText (node : HtmlNode) = node.DirectInnerText()
    let containsText text (node : HtmlNode) = node.InnerText().Contains text

    let rec getNodeStartingWithDirectText (text : string) (element : HtmlNode) =
        if element.DirectInnerText().StartsWith text
        then 
            element
        else 
            element.Elements()
            |> Seq.where (containsText text)
            |> Seq.head
            |> getNodeStartingWithDirectText text

    let url = "https://cs.wiktionary.org/wiki/" + word
    let data = Wiki.Load url
   
    let gender =
        data.Html.Body()
        |> getNodeStartingWithDirectText "rod "
        |> getDirectInnerText

    Gender.FromString gender

let hasGender gender = getGender >> (=) gender
