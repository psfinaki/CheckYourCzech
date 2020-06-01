module Core.Tests.NominalizationTests

open Xunit

open Core.Nouns.Nominalization

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
