module WordTests

open Xunit
open Word

[<Fact>]
let detectsWordIsForTaskPlurals() = 
    "hudba"
    |> isForTaskPlurals
    |> Assert.True

[<Fact>]
let detectsWordIsNotForTaskPluralsNoCzech() = 
    "immigration"
    |> isForTaskPlurals
    |> Assert.False

[<Fact>]
let detectsWordIsNotForTaskPluralsNoCzechNoun() = 
    "koukat"
    |> isForTaskPlurals
    |> Assert.False

[<Fact>]
let detectsWordIsNotForTaskPluralsNoCzechNounDeclension() = 
    "antilopu"
    |> isForTaskPlurals
    |> Assert.False

[<Fact>]
let detectsWordIsForTaskPluralsWithGender() = 
    "gramofon"
    |> isForTaskPluralsWithGender
    |> Assert.True

[<Fact>]
let detectsWordIsNotForTaskPluralsWithGenderNoCzech() = 
    "glass"
    |> isForTaskPluralsWithGender
    |> Assert.False

[<Fact>]
let detectsWordIsNotForTaskPluralsWithGenderNoCzechNoun() = 
    "skvělý"
    |> isForTaskPluralsWithGender
    |> Assert.False

[<Fact>]
let detectsWordIsNotForTaskPluralsWithGenderNoCzechNounDeclension() = 
    "idem"
    |> isForTaskPluralsWithGender
    |> Assert.False

[<Fact>]
let detectsWordIsNotForTaskPluralsWithGenderNoCzechNounGender() = 
    "trosečník"
    |> isForTaskPluralsWithGender
    |> Assert.False

[<Fact>]
let detectsWordIsForTaskComparatives() = 
    "dobrý"
    |> isForTaskComparatives
    |> Assert.True

[<Fact>]
let detectsWordIsNotForTaskComparativesNoCzech() = 
    "emergency"
    |> isForTaskComparatives
    |> Assert.False

[<Fact>]
let detectsWordIsNotForTaskComparativesNoCzechAdjective() = 
    "nazdar"
    |> isForTaskComparatives
    |> Assert.False

[<Fact>]
let detectsWordIsNotForTaskComparativesNoCzechAdjectiveComparison() = 
    "stopkovýtrusý"
    |> isForTaskComparatives
    |> Assert.False

[<Fact>]
let detectsWordIsForTaskImperatives() = 
    "myslet"
    |> isForTaskImperatives
    |> Assert.True

[<Fact>]
let detectsWordIsNotForTaskImperativesNoCzech() = 
    "jump"
    |> isForTaskComparatives
    |> Assert.False

[<Fact>]
let detectsWordIsNotForTaskImperativesNoCzechVerb() = 
    "Lucie"
    |> isForTaskComparatives
    |> Assert.False

[<Fact>]
let detectsWordIsNotForTaskImperativesNoCzechVerbDeclension() = 
    "chlastati"
    |> isForTaskComparatives
    |> Assert.False
