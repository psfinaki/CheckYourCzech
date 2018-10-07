module AdjectiveTests

open Xunit
open Adjective

let equals (x: string[]) (y: string[]) = Assert.Equal<string []>(x, y)

[<Fact>]
let ``Gets comparatives - one option``() = 
    "nový"
    |> getComparatives
    |> equals [|"novější"|]

[<Fact>]
let ``Gets comparatives - mulptiple options``() = 
    "hrubý"
    |> getComparatives
    |> equals [|"hrubší"; "hrubější"|]

[<Fact>]
let ``Validates proper adjective``() =
    "dobrý"
    |> isValid 
    |> Assert.True

[<Fact>]
let ``Invalidates improper adjective - no Czech``() =
    "good"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper adjective - no adjective``() =
    "nazdar"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper adjective - no comparison``() =
    "občasný"
    |> isValid
    |> Assert.False

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
    |> (=) comparative
    |> Assert.True

[<Theory>]
[<InlineData("pomalý", "pomalejší")>]
[<InlineData("drzý", "drzejší")>]
[<InlineData("lysý", "lysejší")>]
let ``Builds theoretical comparative - ejší`` positive comparative =
    positive
    |> buildTheoreticalComparative
    |> (=) comparative
    |> Assert.True
    
[<Theory>]
// TODO: regular adjective ending with "ch"
[<InlineData("chytrý", "chytřejší")>]
[<InlineData("andělský", "andělštější")>]
[<InlineData("africký", "afričtější")>]
[<InlineData("ubohý", "ubožejší")>]
[<InlineData("divoký", "divočejší")>]
[<InlineData("chytrý", "chytřejší")>]
let ``Builds theoretical comparative - stem alternation`` positive comparative =
    positive
    |> buildTheoreticalComparative
    |> (=) comparative
    |> Assert.True

[<Fact>]
let ``Detects regular adjective - stem ends hard``() = 
    "žlutý"
    |> isRegular
    |> Assert.True
    
[<Fact>]
let ``Detects regular adjective - stem ends soft``() = 
    "milý"
    |> isRegular
    |> Assert.True

[<Fact>]
let ``Detects regular adjective - stem alternates``() = 
    "dogmatický"
    |> isRegular
    |> Assert.True

[<Fact>]
let ``Detects irregular adjective``() = 
    "dobrý"
    |> isRegular
    |> Assert.False