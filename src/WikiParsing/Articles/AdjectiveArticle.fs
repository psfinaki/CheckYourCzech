module AdjectiveArticle

open FSharp.Data
open WikiString
open Article

type WikiAdjective = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">
type WikiAdjectiveNový = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">
type WikiAdjectiveStarý = HtmlProvider<"https://cs.wiktionary.org/wiki/starý">
type WikiAdjectiveVrchní = HtmlProvider<"https://cs.wiktionary.org/wiki/vrchní">

let getAdjectiveProvider =
    getUrl
    >> WikiAdjective.Load

let getPluralDobrý = 
    getUrl
    >> WikiAdjectiveNový.Load
    >> fun data -> data.Tables.``Skloňování[editovat]``.Rows.[0].``plurál - mužský životný``

let getPluralStarý = 
    getUrl
    >> WikiAdjectiveStarý.Load
    >> fun data -> data.Tables.``Skloňování[editovat]2``.Rows.[0].``plurál - mužský životný``

let getPluralVrchní = 
    getUrl
    >> WikiAdjectiveVrchní.Load
    >> fun data -> data.Tables.``Skloňování[editovat]3``.Rows.[0].``plurál - mužský životný``

let pluralDeclensionsMap =
    dict [ (1, getPluralDobrý)
           (2, getPluralStarý)
           (3, getPluralVrchní) ]

let getNumberOfDeclensions = 
    getContent
    >> getChildPart "čeština"
    >> getParts "skloňování"
    >> Seq.length

let getPlural adjective = 
    adjective
    |> getNumberOfDeclensions
    |> fun n -> pluralDeclensionsMap.[n] adjective

let getComparatives =
    getAdjectiveProvider
    >> fun data -> data.Tables.``Stupňování[editovat]``.Rows.[1].tvar
    >> getForms
