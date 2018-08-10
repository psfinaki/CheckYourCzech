module Verb

open FSharp.Data

type Wiki = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">

let getImperative word =
    let url = "https://cs.wiktionary.org/wiki/" + word
    let data = Wiki.Load url
    let answer = data.Tables.``Časování[editovat]2``.Rows.[0].``Číslo jednotné - 2.``
    answer.Split "/" |> Array.map (fun s -> s.Trim())
