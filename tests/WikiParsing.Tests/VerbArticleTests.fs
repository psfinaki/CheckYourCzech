module VerbArticleTests

open Xunit
open VerbArticle
open Conjugation

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

[<Fact>]
let ``Gets all conjugations``() =
    let verb =  "myslet"
    
    getConjugation FirstSingular verb  |> equals [|"myslím"|]
    getConjugation SecondSingular verb |> equals [|"myslíš"|]
    getConjugation ThirdSingular verb  |> equals [|"myslí"|]

    getConjugation FirstPlural verb    |> equals [|"myslíme"|]
    getConjugation SecondPlural verb   |> equals [|"myslíte"|]
    getConjugation ThirdPlural verb    |> equals [| "myslí"; "myslejí"|]

[<Fact>]
let ``Removes hovorově conjugation``() = 
    let verb = "jmenuji (hovorově: jmenuju)"
    verb |> removeHovorově |> equals "jmenuji"

[<Fact>]
let ``Keeps hovorově conjugation``() = 
    let verb = "vařit"
    verb |> removeHovorově |> equals verb
