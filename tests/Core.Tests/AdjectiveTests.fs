module AdjectiveTests

open Xunit
open Adjective

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Fact>]
let ``Gets positive - when positive``() = 
    "dobrý"
    |> getPositive
    |> equals "dobrý"

[<Fact>]
let ``Gets positive - when comparative``() = 
    "lepší"
    |> getPositive
    |> equals "dobrý"

[<Fact>]
let ``Gets positive - when superlative``() = 
    "nejlepší"
    |> getPositive
    |> equals "dobrý"

[<Fact>]
let ``Detects syntactic comparison``() = 
    "více hyzdící"
    |> isSyntacticComparison
    |> Assert.True

[<Fact>]
let ``Detects morphological comparison``() = 
    "novější"
    |> isSyntacticComparison
    |> Assert.False

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

[<Fact>]
let ``Invalidates improper adjective - not a positive form - comparative``() =
    "horší"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper adjective - not a positive form - superlative``() =
    "nejširší"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper adjective - no comparatives``() =
    "optimální"
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
let ``Detects regular adjective - no comparative``() = 
    "vroucí"
    |> isRegular
    |> Assert.True

[<Fact>]
let ``Detects irregular adjective``() = 
    "dobrý"
    |> isRegular
    |> Assert.False
