module ComparativeTests

open Xunit
open Comparative

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Theory>]
[<InlineData("rudý", "rudější")>]
[<InlineData("důležitý", "důležitější")>]
[<InlineData("krásný", "krásnější")>]
[<InlineData("pečlivý", "pečlivější")>]
[<InlineData("známý", "známější")>]
[<InlineData("hrubý", "hrubější")>]
[<InlineData("hloupý", "hloupější")>]
let ``Builds theoretical comparative - ější`` positive comparative =
    positive
    |> buildTheoreticalComparative
    |> equals (Some comparative)

[<Theory>]
[<InlineData("pomalý", "pomalejší")>]
[<InlineData("drzý", "drzejší")>]
[<InlineData("lysý", "lysejší")>]
let ``Builds theoretical comparative - ejší`` positive comparative =
    positive
    |> buildTheoreticalComparative
    |> equals (Some comparative)

[<Theory>]
// TODO: regular adjective ending with "ch"
[<InlineData("chytrý", "chytřejší")>]
[<InlineData("andělský", "andělštější")>]
[<InlineData("africký", "afričtější")>]
[<InlineData("tenoučký", "tenoučtější")>]
[<InlineData("ubohý", "ubožejší")>]
[<InlineData("divoký", "divočejší")>]
let ``Builds theoretical comparative - stem alternation`` positive comparative =
    positive
    |> buildTheoreticalComparative
    |> equals (Some comparative)

[<Fact>]
let ``Detects when cannot build theoretical comparative``() =
    "vroucí"
    |> buildTheoreticalComparative
    |> equals None
