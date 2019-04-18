module WordTests

open Xunit
open Word

[<Fact>]
let ``Detects noun``() =
    "planeta"
    |> isNoun
    |> Assert.True

[<Fact>]
let ``Detects not a noun - no Czech part``() =
    "good"
    |> isNoun
    |> Assert.False

[<Fact>]
let ``Detects not a noun - no noun part``() =
    "spát"
    |> isNoun
    |> Assert.False

[<Fact>]
let ``Detects adjective``() =
    "nový"
    |> isAdjective
    |> Assert.True

[<Fact>]
let ``Detects not an adjective``() =
    "les"
    |> isAdjective
    |> Assert.False

[<Fact>]
let ``Detects verb``() =
    "milovat"
    |> isVerb
    |> Assert.True

[<Fact>]
let ``Detects not a verb``() =
    "kus"
    |> isVerb
    |> Assert.False
