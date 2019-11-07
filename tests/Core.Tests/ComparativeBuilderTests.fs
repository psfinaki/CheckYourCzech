module ComparativeBuilderTests

open Xunit
open ComparativeBuilder

[<Theory>]
[<InlineData("nový", "nov")>]
[<InlineData("moderní", "modern")>]
let ``Gets stem`` adjective stem =
    adjective
    |> getStem
    |> equals stem
