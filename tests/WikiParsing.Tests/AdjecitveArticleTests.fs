module AjdectiveArticleTests

open Xunit
open AdjectiveArticle

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Fact>]
let ``Gets plural - when not nominalized``() = 
    "laskavý"
    |> getPlural
    |> equals "laskaví"

[<Fact>]
let ``Gets plural - when nominalized once``() = 
    "vrátný"
    |> getPlural
    |> equals "vrátní"

[<Fact>]
let ``Gets plural - when nominalized twice``() = 
    "duchovní"
    |> getPlural
    |> equals "duchovní"

[<Fact>]
let ``Gets comparatives``() = 
    "dobrý"
    |> getComparatives
    |> equals [| "lepší" |]

[<Fact>]
let ``Gets number of declensions``() = 
    "nový"
    |> getNumberOfDeclensions
    |> equals 1

