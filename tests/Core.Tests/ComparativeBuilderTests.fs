module Core.Tests.ComparativeBuilderTests

open Xunit

open Core.Adjectives.ComparativeBuilder

[<Theory>]
[<InlineData("nový", "nov")>]
[<InlineData("moderní", "modern")>]
let ``Gets stem`` adjective stem =
    adjective
    |> getStem
    |> equals stem
