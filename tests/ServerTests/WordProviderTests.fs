namespace ServerTests

open ServerCode
open Xunit

module WordProviderTests =

    [<Fact>]
    let detectsCzechNoun() = 
        "hudba"
        |> WordProvider.isProperNoun
        |> Assert.True

    [<Fact>]
    let detectsNoCzechPart() = 
        "immigration"
        |> WordProvider.isProperNoun
        |> Assert.False

    [<Fact>]
    let detectsNoCzechNoun() = 
        "koukat"
        |> WordProvider.isProperNoun
        |> Assert.False

    [<Fact>]
    let detectsNoCzechDeclination() = 
        "antilopu"
        |> WordProvider.isProperNoun
        |> Assert.False

    [<Fact>]
    let detectsPlural() =
        "panda"
        |> WordProvider.hasPlural
        |> Assert.True

    [<Fact>]
    let detectsNoPlural() = 
        "Oxford"
        |> WordProvider.hasPlural
        |> Assert.False        