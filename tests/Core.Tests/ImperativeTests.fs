module ImperativeTests

open Xunit
open Imperative
open System

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

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
let ``Invalidates improper verb - no imperative``() =
    "chlastati"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper verb - archaic``() =
    "péci"
    |> isValid
    |> Assert.False

[<Theory>]
[<InlineData("dělá", 5)>]
[<InlineData("prosí", 4)>]
[<InlineData("kupuje", 3)>]
[<InlineData("praskne", 2)>]
[<InlineData("nése", 1)>]
let ``Gets class by third person singular`` ``third person singular`` ``class`` =
    ``third person singular``
    |> getClassByThirdPersonSingular
    |> equals ``class``

[<Fact>]
let ``Throws for invalid third person singular``() =
    let action () = getClassByThirdPersonSingular "test" |> ignore
    Assert.Throws<ArgumentException> action

[<Theory>]
[<InlineData("řídit", 4)>]
[<InlineData("řídit se", 4)>]
let ``Gets class`` verb ``class`` =
    verb
    |> getClass
    |> equals ``class``
