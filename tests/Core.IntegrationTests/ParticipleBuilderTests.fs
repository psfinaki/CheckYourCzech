module ParticipleBuilderTests

open Xunit
open ParticipleBuilder
open System

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Theory>]
[<InlineData("dělat", "dělal")>]
[<InlineData("tisknout", "tiskl")>]
[<InlineData("minout", "minul")>]
let ``Builds participle for non-reflexive`` infinitive participle =
    infinitive
    |> buildParticipleNonReflexive
    |> equals participle

[<Theory>]
[<InlineData("starat se", "staral se")>]
[<InlineData("vážit si", "vážil si")>]
let ``Builds participle for reflexive`` infinitive participle =
    infinitive
    |> buildParticipleReflexive
    |> equals participle

[<Fact>]
let ``Throws for invalid reflexive``() =
    let action () = buildParticipleReflexive "test" |> ignore
    Assert.Throws<ArgumentException> action

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
