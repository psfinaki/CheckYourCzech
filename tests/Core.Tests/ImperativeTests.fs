module ImperativeTests

open Xunit
open Imperative

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

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

[<Fact>]
let ``Invalidates improper verb - no conjugation``() =
    "chlastati"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper verb - no imperative``() =
    "moct"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper verb - archaic``() =
    "péci"
    |> isValid
    |> Assert.False
