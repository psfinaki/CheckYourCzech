module Core.Tests.ArchaismsTests

open Xunit

open Core.Verbs.Archaisms

[<Theory>]
[<InlineData "housti">]
[<InlineData "péci">]
let ``Detects archaic ending`` verb =
    verb
    |> hasArchaicEnding
    |> Assert.True

[<Fact>]
let ``Detects modern ending``() =
    "péct"
    |> hasArchaicEnding
    |> Assert.False

