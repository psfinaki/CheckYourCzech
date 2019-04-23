module ParticipleTests

open Xunit
open Participle

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Fact>]
let ``Detects regular participle - pattern tisknout``() = 
    "sednout"
    |> isRegular
    |> Assert.True

[<Fact>]
let ``Detects regular participle - pattern minout``() = 
    "minout"
    |> isRegular
    |> Assert.True

[<Fact>]
let ``Detects regular participle - common pattern``() = 
    "dělat"
    |> isRegular
    |> Assert.True

[<Fact>]
let ``Detects irregular participle``() = 
    "jít"
    |> isRegular
    |> Assert.False
    
