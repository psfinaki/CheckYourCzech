module WikiParsing.Articles.AdjectiveArticle

open FSharp.Data

open WikiParsing.WikiString
open WikiParsing.Articles.Article
open Common.WikiArticles

type WikiAdjective = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">
type WikiAdjectiveNový = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">
type WikiAdjectiveStarý = HtmlProvider<"https://cs.wiktionary.org/wiki/starý">
type WikiAdjectiveVrchní = HtmlProvider<"https://cs.wiktionary.org/wiki/vrchní">

type AdjectiveArticleWithPlural = AdjectiveArticleWithPlural of AdjectiveArticle
type AdjectiveArticleWithComparative = AdjectiveArticleWithComparative of AdjectiveArticle

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

let private hasRequiredInfoPlural (AdjectiveArticle article) = 
    article |> isMatch [
        Is "přídavné jméno"
        Is "skloňování"
    ]

let private hasRequiredInfoComparative (AdjectiveArticle article) =
    article |> isMatch [
        Is "přídavné jméno"
        Is "stupňování"
    ]

let getPlural article =
    article
    |> getNumberOfDeclensions
    |> fun n -> pluralDeclensionsMap.[n] article

let getComparatives (AdjectiveArticleWithComparative article) =
    article
    |> getAdjectiveProvider
    |> fun data -> data.Tables.``Stupňování[editovat]``.Rows.[1].tvar
    |> getForms

let parseAdjectivePlural article = 
    if article |> hasRequiredInfoPlural
    then Some (AdjectiveArticleWithPlural article)
    else None

let parseAdjectiveComparative article =
    if article |> hasRequiredInfoComparative
    then Some (AdjectiveArticleWithComparative article)
    else None

let parseAdjectiveArticle article =
    let (AdjectiveArticle { Title = title }) = article
    {
        CanonicalForm = title
        Declension =
            article
            |> parseAdjectivePlural
            |> Option.map (fun article -> { 
                Singular = title
                Plural = article |> getPlural 
            })
        Comparison = 
            article
            |> parseAdjectiveComparative
            |> Option.map (fun article -> {
                Positive = title
                Comparatives = article |> getComparatives
            })
    }
