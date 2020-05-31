module WikiParsing.Articles.AdjectiveArticle

open FSharp.Data

open WikiParsing.WikiString
open Article
open Common.WikiArticles

type WikiAdjective = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">
type WikiAdjectiveNový = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">
type WikiAdjectiveStarý = HtmlProvider<"https://cs.wiktionary.org/wiki/starý">
type WikiAdjectiveVrchní = HtmlProvider<"https://cs.wiktionary.org/wiki/vrchní">

let getAdjectiveProvider (AdjectiveArticle { Text = text }) =
    text
    |> WikiAdjective.Parse

let getPluralDobrý (AdjectiveArticleWithPlural (AdjectiveArticle { Text = text })) = 
    text
    |> WikiAdjectiveNový.Parse
    |> fun data -> data.Tables.``Skloňování[editovat]``.Rows.[0].``plurál - mužský životný``

let getPluralStarý (AdjectiveArticleWithPlural (AdjectiveArticle { Text = text })) = 
    text
    |> WikiAdjectiveStarý.Parse
    |> fun data -> data.Tables.``Skloňování[editovat]2``.Rows.[0].``plurál - mužský životný``

let getPluralVrchní (AdjectiveArticleWithPlural (AdjectiveArticle { Text = text })) = 
    text
    |> WikiAdjectiveVrchní.Parse
    |> fun data -> data.Tables.``Skloňování[editovat]3``.Rows.[0].``plurál - mužský životný``

let pluralDeclensionsMap =
    dict [ (1, getPluralDobrý)
           (2, getPluralStarý)
           (3, getPluralVrchní) ]

let getNumberOfDeclensions (AdjectiveArticleWithPlural (AdjectiveArticle article)) = 
    article
    |> matches [
        Any
        Is "skloňování"
    ]
    |> Seq.length

let getPlural article =
    article
    |> getNumberOfDeclensions
    |> fun n -> pluralDeclensionsMap.[n] article

let getComparatives (AdjectiveArticleWithComparative article) =
    article
    |> getAdjectiveProvider
    |> fun data -> data.Tables.``Stupňování[editovat]``.Rows.[1].tvar
    |> getForms
