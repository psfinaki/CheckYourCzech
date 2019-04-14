module ImperativeTests

open Xunit
open Imperative

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Fact>]
let ``Validates proper verb``() =
    "spát"
    |> isValid 
    |> Assert.True
