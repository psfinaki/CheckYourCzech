module VerbTests

open Xunit
open Verb

[<Fact>]
let ``Detects imperatives present``() = 
    "myslet"
    |> hasImperatives
    |> Assert.True

[<Fact>]
let ``Detects imperatives absent``() = 
    "musit"
    |> hasImperatives
    |> Assert.False
    
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