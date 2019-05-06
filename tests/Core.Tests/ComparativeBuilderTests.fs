module ComparativeBuilderTests

open Xunit
open ComparativeBuilder

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Theory>]
[<InlineData("nový", "nov")>]
[<InlineData("moderní", "modern")>]
let ``Gets stem`` adjective stem =
    adjective
    |> getStem
    |> equals stem
