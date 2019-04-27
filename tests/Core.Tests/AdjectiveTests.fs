module AdjectiveTests

open Xunit
open Adjective

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Fact>]
let ``Detects syntactic comparison``() = 
    "více hyzdící"
    |> isSyntacticComparison
    |> Assert.True

[<Fact>]
let ``Detects morphological comparison``() = 
    "novější"
    |> isSyntacticComparison
    |> Assert.False
