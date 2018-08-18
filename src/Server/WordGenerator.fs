module WordGenerator

open FSharp.Data

type WikiArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">

let randomWikiArticleUrl = "https://cs.wiktionary.org/wiki/Speciální:Náhodná_stránka"

let getRandomWord() =
    randomWikiArticleUrl
    |> WikiArticle.Load 
    |> fun data -> data.Html
    |> Article.getNameFromHtml
    