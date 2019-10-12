module VerbClassesTests

open System
open Xunit
open VerbClasses

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

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
