module AdjectiveArticleTests

open Xunit
open AdjectiveArticle
open WikiArticles

let getArticle = 
    Article.getArticle
    >> Option.get
    >> AdjectiveArticle

[<Fact>]
let ``Gets plural - when not nominalized``() = 
    "laskavý"
    |> getArticle 
    |> AdjectiveArticleWithPlural
    |> getPlural
    |> equals "laskaví"

[<Fact>]
let ``Gets plural - when nominalized once``() = 
    "vrátný"
    |> getArticle 
    |> AdjectiveArticleWithPlural
    |> getPlural
    |> equals "vrátní"

[<Fact>]
let ``Gets plural - when nominalized twice``() = 
    "duchovní"
    |> getArticle 
    |> AdjectiveArticleWithPlural
    |> getPlural
    |> equals "duchovní"

[<Fact>]
let ``Gets comparatives``() = 
    "dobrý"
    |> getArticle 
    |> AdjectiveArticleWithComparative
    |> getComparatives
    |> equals [| "lepší" |]

[<Fact>]
let ``Gets number of declensions``() = 
    "nový"
    |> getArticle 
    |> AdjectiveArticleWithPlural
    |> getNumberOfDeclensions
    |> equals 1

