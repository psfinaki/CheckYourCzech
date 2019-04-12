module ParticipleTests

open Xunit
open Participle

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Fact>]
let ``Gets participle from the second table``() = 
    "uvidět"
    |> getParticiplesTable2
    |> equals "uviděl"

[<Fact>]
let ``Gets participle from the third table``() = 
    "myslet"
    |> getParticiplesTable3
    |> equals "myslel"

[<Fact>]
let ``Validates proper verb``() =
    "spát"
    |> isValid 
    |> Assert.True

[<Fact>]
let ``Invalidates improper verb - no Czech``() =
    "good"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper verb - no verb``() =
    "nazdar"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper verb - archaic``() =
    "péci"
    |> isValid
    |> Assert.False

[<Theory>]
[<InlineData("dělat", "dělal")>]
[<InlineData("tisknout", "tiskl")>]
[<InlineData("minout", "minul")>]
[<InlineData("starat se", "staral se")>]
let ``Builds theoretical participle`` infinitive participle =
    infinitive
    |> buildTheoreticalParticiple
    |> equals participle

[<Fact>]
let ``Detects regular participle - pattern tisknout``() = 
    "sednout"
    |> isRegular
    |> Assert.True

[<Fact>]
let ``Detects regular participle - pattern minout``() = 
    "minout"
    |> isRegular
    |> Assert.True

[<Fact>]
let ``Detects regular participle - common pattern``() = 
    "dělat"
    |> isRegular
    |> Assert.True

[<Fact>]
let ``Detects irregular participle``() = 
    "jít"
    |> isRegular
    |> Assert.False

[<Fact>]
let ``Builds participle for pattern tisknout``() =
    "tisknout"
    |> buildParticipleTisknout
    |> equals "tiskl"

[<Fact>]
let ``Builds participle for pattern minout``() =
    "minout"
    |> buildParticipleMinout
    |> equals "minul"

[<Fact>]
let ``Builds participle for common pattern``() =
    "dělat"
    |> buildParticipleCommon
    |> equals "dělal"

[<Fact>]
let ``Gets reflexive - se``() = 
    "starat se"
    |> getReflexive
    |> equals (Some "se")

[<Fact>]
let ``Gets reflexive - si``() = 
    "vážit si"
    |> getReflexive
    |> equals (Some "si")

[<Fact>]
let ``Detects no reflexive``() = 
    "spát"
    |> getReflexive
    |> equals None

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
