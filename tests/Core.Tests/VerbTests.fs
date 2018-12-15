module VerbTests

open Xunit
open Verb

[<Theory>]
[<InlineData "housti">]
[<InlineData "péci">]
let ``Detects archaic verb`` verb =
    verb
    |> isArchaic
    |> Assert.True

[<Fact>]
let ``Detects modern verb``() =
    "péct"
    |> isArchaic
    |> Assert.False
