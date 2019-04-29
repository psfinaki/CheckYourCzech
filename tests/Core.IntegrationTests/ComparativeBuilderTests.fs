module ComparativeBuilderTests

open Xunit
open System
open ComparativeBuilder

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
    |> buildComparative
    |> equals comparative

[<Theory>]
[<InlineData("pomalý", "pomalejší")>]
[<InlineData("drzý", "drzejší")>]
[<InlineData("lysý", "lysejší")>]
let ``Builds theoretical comparative - ejší`` positive comparative =
    positive
    |> buildComparative
    |> equals comparative

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
    |> buildComparative
    |> equals comparative

[<Fact>]
let ``Detects when can build theoretical comparative``() =
    "dobrý"
    |> canBuildComparative
    |> Assert.True

[<Fact>]
let ``Detects when cannot build theoretical comparative``() =
    "vroucí"
    |> canBuildComparative
    |> Assert.False

[<Theory>]
[<InlineData("divoč", "divočejší")>]
[<InlineData("hrub", "hrubější")>]
let ``Adds comparative suffix`` stem adjective =
    stem
    |> addComparativeSuffix
    |> equals adjective

[<Fact>]
let ``Throws for invalid stem``() =
    let action () = addComparativeSuffix "bla" |> ignore
    Assert.Throws<ArgumentException> action
