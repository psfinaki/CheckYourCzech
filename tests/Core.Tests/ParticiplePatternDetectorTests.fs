module Core.Tests.ParticiplePatternDetectorTests

open Xunit

open Core.Verbs.ParticiplePatternDetector
open Common.GrammarCategories.Verbs

[<Fact>]
let ``Detects pattern tisknout``() =
    "lehnout"
    |> getPattern
    |> equals ParticiplePattern.Tisknout

[<Fact>]
let ``Detects pattern minout``() =
    "plynout"
    |> getPattern
    |> equals ParticiplePattern.Minout

[<Fact>]
let ``Detects common pattern``() =
    "prosit"
    |> getPattern
    |> equals ParticiplePattern.Common
