module Core.Tests.ParticiplePatternDetectorTests

open Xunit

open Core.Verbs.ParticiplePatternDetector
open Common.Verbs

[<Fact>]
let ``Detects pattern tisknout``() =
    "lehnout"
    |> getPattern
    |> equals Tisknout

[<Fact>]
let ``Detects pattern minout``() =
    "plynout"
    |> getPattern
    |> equals Minout

[<Fact>]
let ``Detects common pattern``() =
    "prosit"
    |> getPattern
    |> equals Common
