module VerbPatternDetectorTests

open Xunit
open VerbPatternDetector

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Fact>]
let ``Gets pattern``() =
    getPattern "ohlásit"
    |> equals (Some "prosit")
