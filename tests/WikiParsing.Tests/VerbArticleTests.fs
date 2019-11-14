module VerbArticleTests

open Xunit
open VerbArticle

[<Fact>]
let ``Gets imperatives - single option``() =
    "milovat"
    |> getImperatives
    |> equals [|"miluj"|]

[<Fact>]
let ``Gets imperatives - multiple options``() =
    "orat"
    |> getImperatives
    |> equals [|"oř"; "orej"|]

[<Fact>]
let ``Gets participle from the second table``() = 
    "uvidět"
    |> getParticiples
    |> equals [|"uviděl"|]

[<Fact>]
let ``Gets participle from the third table``() = 
    "myslet"
    |> getParticiples
    |> equals [|"myslel"|]
