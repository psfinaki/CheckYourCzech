module AdjectiveTests

open Xunit
open Adjective

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Fact>]
let ``Gets plural``() = 
    "dobrý"
    |> getPlural
    |> equals "dobří"

[<Fact>]
let ``Gets positive - when positive``() = 
    "dobrý"
    |> getPositive
    |> equals "dobrý"

[<Fact>]
let ``Gets positive - when comparative``() = 
    "lepší"
    |> getPositive
    |> equals "dobrý"

[<Fact>]
let ``Gets positive - when superlative``() = 
    "nejlepší"
    |> getPositive
    |> equals "dobrý"

[<Fact>]
let ``Gets comparatives``() = 
    "dobrý"
    |> getComparatives
    |> equals [| "lepší" |]

[<Fact>]
let ``Detects comparison``() =
    "dobrý"
    |> hasComparison
    |> Assert.True

[<Fact>]
let ``Detects no comparison``() =
    "první"
    |> hasComparison
    |> Assert.False

[<Fact>]
let ``Detects positive adjective``() =
    "dobrý"
    |> isPositive
    |> Assert.True

[<Theory>]
[<InlineData "lepší">]
[<InlineData "nejširší">]
let ``Detects not positive adjective`` adjective =
    adjective
    |> isPositive
    |> Assert.False

[<Fact>]
let ``Detects morphological comparatives``() =
    "dobrý"
    |> hasMorphologicalComparatives
    |> Assert.True

[<Fact>]
let ``Detects no morphological comparatives - no comparatives``() =
    "optimální"
    |> hasMorphologicalComparatives
    |> Assert.False

[<Fact>]
let ``Detects no morphological comparatives - no morphologicals``() =
    "vroucí"
    |> hasMorphologicalComparatives
    |> Assert.False
