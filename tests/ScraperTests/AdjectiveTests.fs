module AdjectiveTests

open Xunit
open Adjective

let equals (x: string[]) (y: string[]) = Assert.Equal<string []>(x, y)

[<Fact>]
let ``Gets comparatives - one option``() = 
    "nový"
    |> getComparatives
    |> equals [|"novější"|]

[<Fact>]
let ``Gets comparatives - mulptiple options``() = 
    "hrubý"
    |> getComparatives
    |> equals [|"hrubší"; "hrubější"|]

[<Fact>]
let ``Validates proper adjective``() =
    "dobrý"
    |> isValid 
    |> Assert.True

[<Fact>]
let ``Invalidates improper adjective - no Czech``() =
    "good"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper adjective - no adjective``() =
    "nazdar"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper adjective - no comparison``() =
    "občasný"
    |> isValid
    |> Assert.False