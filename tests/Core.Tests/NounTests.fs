module NounTests

open Xunit
open Noun
open Gender

[<Fact>]
let ``Gets gender masculine animate``() =
    "tata"
    |> getGender
    |> (=) MasculineAnimate
    |> Assert.True

[<Fact>]
let ``Gets gender masculine inanimate``() =
    "hrad"
    |> getGender
    |> (=) MasculineInanimate
    |> Assert.True

[<Fact>]
let ``Gets gender feminine``() =
    "panda"
    |> getGender
    |> (=) Feminine
    |> Assert.True

[<Fact>]
let ``Gets gender neuter``() =
    "okno"
    |> getGender
    |> (=) Neuter
    |> Assert.True

[<Fact>]
let ``Gets wiki plural - common article``() = 
    "panda"
    |> getWikiPlural
    |> (=) "pandy"

[<Fact>]
let ``Gets wiki plural - locked article``() = 
    "debil"
    |> getWikiPlural
    |> (=) "debilové"

[<Fact>]
let ``Validates proper noun``() =
    "panda"
    |> isValid 
    |> Assert.True

[<Fact>]
let ``Invalidates improper noun - no Czech``() =
    "good"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper noun - no noun``() =
    "koukat"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper noun - no declension``() =
    "antilopu"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper noun - no gender``() =
    "trosečník"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper noun - incorrect gender``() =
    "benediktin"
    |> isValid
    |> Assert.False