namespace ServerTests

open Xunit
open WordProvider

module WordProviderTests =

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
    let detectsNoCzechDeclination() = 
        "antilopu"
        |> isProperNoun
        |> Assert.False

    [<Fact>]
    let detectsPlural() =
        "panda"
        |> hasPlural
        |> Assert.True

    [<Fact>]
    let detectsNoPlural() = 
        "Oxford"
        |> hasPlural
        |> Assert.False        