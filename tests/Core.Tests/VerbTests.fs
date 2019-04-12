module VerbTests

open System
open Xunit
open Verb

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Theory>]
[<InlineData "housti">]
[<InlineData "péci">]
let ``Detects archaic ending`` verb =
    verb
    |> hasArchaicEnding
    |> Assert.True

[<Fact>]
let ``Detects modern ending``() =
    "péct"
    |> hasArchaicEnding
    |> Assert.False

[<Theory>]
[<InlineData("starat se", "starat")>]
[<InlineData("vážit si", "vážit")>]
[<InlineData("spát", "spát")>]
let ``Removes reflexive`` verb nonReflexive = 
    verb
    |> removeReflexive
    |> equals nonReflexive

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
    |> equals (Some ``class``)
