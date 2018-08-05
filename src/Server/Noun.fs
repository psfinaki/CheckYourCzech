module Noun

open FSharp.Data
open Gender
open Article

type Wiki = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let getPlural word = 
    let url = "https://cs.wiktionary.org/wiki/" + word
    let data = Wiki.Load url
    let answer = data.Tables.``Skloňování[editovat]``.Rows.[0].plurál
    
    match answer with
    | "—" | "" -> [||]
    | _ -> answer.Split "/" |> Array.map (fun s -> s.Trim()) 

let hasPlural = getPlural >> (not << Array.isEmpty)

let getGender =
    Article.getContent
    >> getPart "čeština"
    >> getPart "podstatné jméno"
    >> getInfo "rod"
    >> Gender.FromString

let hasGender gender = getGender >> (=) gender
