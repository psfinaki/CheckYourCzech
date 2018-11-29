module ParticipleTests

open Xunit
open Participle

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Fact>]
let ``Gets participle from the second table``() = 
    "uvidět"
    |> getParticiplesTable2
    |> equals "uviděl"

[<Fact>]
let ``Gets participle from the third table``() = 
    "myslet"
    |> getParticiplesTable3
    |> equals "myslel"

[<Fact>]
let ``Validates proper verb``() =
    "spát"
    |> isValid 
    |> Assert.True

[<Fact>]
let ``Invalidates improper verb - no Czech``() =
    "good"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper verb - no verb``() =
    "nazdar"
    |> isValid
    |> Assert.False
