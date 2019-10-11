module NounTests

open Xunit
open Noun

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Theory>]
[<InlineData "Hrabovský">]
[<InlineData "Abrahámová">]
[<InlineData "znalečné">]
[<InlineData "hovězí">]
let ``Detects nominalization`` noun =
    noun
    |> isNominalization
    |> Assert.True

[<Fact>]
let ``Detects no nominalization``() =
    "láska"
    |> isNominalization
    |> Assert.False

[<Fact>]
let ``Detects particles``() =
    "prstění se"
    |> hasParticles
    |> Assert.True

[<Fact>]
let ``Detects no particles``() =
    "slunce"
    |> hasParticles
    |> Assert.False
