module AccusativeTests

open Xunit
open Accusative

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Fact>]
let ``Validates proper noun``() =
    "panda"
    |> isValid 
    |> Assert.True
