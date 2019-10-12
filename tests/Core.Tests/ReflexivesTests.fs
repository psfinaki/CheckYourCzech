module ReflexivesTests

open Xunit
open Reflexives
open System

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Theory>]
[<InlineData "starat se">]
[<InlineData "vážit si">]
let ``Detects reflexive verb`` verb =
    verb
    |> isReflexive
    |> Assert.True

[<Fact>]
let ``Detects non-reflexive verb``() =
    "milovat"
    |> isReflexive
    |> Assert.False

[<Theory>]
[<InlineData("starat se", "starat")>]
[<InlineData("vážit si", "vážit")>]
[<InlineData("spát", "spát")>]
let ``Removes reflexive`` verb nonReflexive = 
    verb
    |> removeReflexive
    |> equals nonReflexive

[<Fact>]
let ``Gets reflexive - se``() = 
    "starat se"
    |> getReflexive
    |> equals "se"

[<Fact>]
let ``Gets reflexive - si``() = 
    "vážit si"
    |> getReflexive
    |> equals "si"

[<Fact>]
let ``Detects no reflexive``() =
    let action () = getReflexive "spát" |> ignore
    Assert.Throws<ArgumentException> action
