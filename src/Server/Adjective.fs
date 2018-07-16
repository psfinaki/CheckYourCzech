module Adjective

open FSharp.Data

type Wiki = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">

let getComparative word =
    let url = "https://cs.wiktionary.org/wiki/" + word
    let data = Wiki.Load url
    let answer = data.Tables.``Stupňování[editovat]``.Rows.[1].tvar
    answer.Split "/" |> Array.map (fun s -> s.Trim())
