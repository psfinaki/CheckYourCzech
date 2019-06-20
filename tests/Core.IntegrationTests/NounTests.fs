module NounTests

open Xunit
open Noun
open Genders

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Fact>]
let ``Gets gender masculine animate``() =
    "tata"
    |> getGender
    |> equals MasculineAnimate

[<Fact>]
let ``Gets gender masculine inanimate``() =
    "hrad"
    |> getGender
    |> equals MasculineInanimate

[<Fact>]
let ``Gets gender feminine``() =
    "panda"
    |> getGender
    |> equals Feminine

[<Fact>]
let ``Gets gender neuter``() =
    "okno"
    |> getGender
    |> equals Neuter
    
[<Fact>]
let ``Detects indeclinable``() =
    "dada"
    |> isIndeclinable
    |> Assert.True
    
[<Fact>]
let ``Detects declinable``() =
    "panda"
    |> isIndeclinable
    |> Assert.False

[<Fact>]
let ``Detects declension``() =
    "panda"
    |> hasDeclension
    |> Assert.True

[<Fact>]
let ``Detects no declension``() =
    "antilopu"
    |> hasDeclension
    |> Assert.False

[<Fact>]
let ``Detects gender``() =
    "panda"
    |> hasGender
    |> Assert.True

[<Fact>]
let ``Detects no gender - gender not specified``() =
    "trosečník"
    |> hasGender
    |> Assert.False

[<Fact>]
let ``Detects no gender - incorrect gender``() =
    "benediktin"
    |> hasGender
    |> Assert.False
