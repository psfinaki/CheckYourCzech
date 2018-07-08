module WordTests

open Xunit
open Word

[<Fact>]
let detectsCzechNoun() = 
    "hudba"
    |> isNoun
    |> Assert.True

[<Fact>]
let detectsNoCzechPart() = 
    "immigration"
    |> isNoun
    |> Assert.False

[<Fact>]
let detectsNoCzechNoun() = 
    "koukat"
    |> isNoun
    |> Assert.False

[<Fact>]
let detectsNoCzechDeclension() = 
    "antilopu"
    |> isNoun
    |> Assert.False

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
