module AdjectiveComparativeTests

open Xunit
open AdjectiveComparative

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Fact>]
let ``Detects regular adjective - stem ends hard``() = 
    "žlutý"
    |> isRegular
    |> Assert.True
    
[<Fact>]
let ``Detects regular adjective - stem ends soft``() = 
    "milý"
    |> isRegular
    |> Assert.True

[<Fact>]
let ``Detects regular adjective - stem alternates``() = 
    "dogmatický"
    |> isRegular
    |> Assert.True

[<Fact>]
let ``Detects irregular adjective``() = 
    "dobrý"
    |> isRegular
    |> Assert.False
