module WordTests

open Xunit
open Word

[<Fact>]
let detectsCzechNoun() = 
    "hudba"
    |> isProperNoun
    |> Assert.True

[<Fact>]
let detectsNoCzechPart() = 
    "immigration"
    |> isProperNoun
    |> Assert.False

[<Fact>]
let detectsNoCzechNoun() = 
    "koukat"
    |> isProperNoun
    |> Assert.False

[<Fact>]
let detectsNoCzechDeclension() = 
    "antilopu"
    |> isProperNoun
    |> Assert.False

[<Fact>]
let detectsPlural() =
    "panda"
    |> hasPlural
    |> Assert.True

[<Fact>]
let detectsNoPlural1() = 
    "Oxford"
    |> hasPlural
    |> Assert.False

[<Fact>]
let detectsNoPlural2() = 
    "Pegas"
    |> hasPlural
    |> Assert.False
        
[<Fact>]
let detectsGender() =
    "panda"
    |> hasGender Gender.Feminine
    |> Assert.True

[<Fact>]
let detectsCzechAdjective() = 
    "dobrý"
    |> isAdjective
    |> Assert.True

[<Fact>]
let detectsNoCzechAdjective() = 
    "nazdar"
    |> isAdjective
    |> Assert.False
