module WikiParsing.Tests.AdjectiveArticleTests

open Xunit

open Common.WikiArticles
open WikiParsing.Articles.AdjectiveArticle
open WikiParsing.ConcreteArticles

let getArticle = 
    getArticle
    >> AdjectiveArticle

[<Fact>]
let ``Gets plural - when not nominalized``() = 
    "laskavý"
    |> getArticle 
    |> AdjectiveArticleWithPlural
    |> getDeclension
    |> fun d -> d.Plural
    |> equals "laskaví"

[<Fact>]
let ``Gets plural - when nominalized once``() = 
    "vrátný"
    |> getArticle 
    |> AdjectiveArticleWithPlural
    |> getDeclension
    |> fun d -> d.Plural
    |> equals "vrátní"

[<Fact>]
let ``Gets plural - when nominalized twice``() = 
    "duchovní"
    |> getArticle 
    |> AdjectiveArticleWithPlural
    |> getDeclension
    |> fun d -> d.Plural
    |> equals "duchovní"

[<Fact>]
let ``Gets comparatives``() = 
    "dobrý"
    |> getArticle 
    |> AdjectiveArticleWithComparative
    |> getComparison
    |> fun d -> d.Comparatives
    |> seqEquals [ "lepší" ]

[<Fact>]
let ``Gets number of declensions``() = 
    "nový"
    |> getArticle 
    |> AdjectiveArticleWithPlural
    |> getNumberOfDeclensions
    |> equals 1

[<Theory>]
[<InlineData "nový">]
[<InlineData "starý">]
let ``Detects valid word for plurals`` word =
    word
    |> getArticle 
    |> parseAdjectivePlural
    |> Option.isSome
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "láska">]
let ``Detects invalid word for plurals`` word =
    word
    |> getArticle 
    |> parseAdjectivePlural
    |> Option.isSome
    |> Assert.False

[<Theory>]
[<InlineData "nový">]
[<InlineData "starý">]
let ``Detects valid word for comparatives`` word =
    word
    |> getArticle 
    |> parseAdjectiveComparative
    |> Option.isSome
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "láska">]
let ``Detects invalid word for comparatives`` word =
    word
    |> getArticle 
    |> parseAdjectiveComparative
    |> Option.isSome
    |> Assert.False
