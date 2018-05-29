namespace ServerTests

open ServerCode
open Xunit

module WordProviderTests =

    [<Fact>]
    let detectsCzechNoun() = 
        let word = "hudba"

        let result = WordProvider.isProperNoun word
        
        Assert.True result

    [<Fact>]
    let detectsNoCzechPart() = 
        let word = "immigration"

        let result = WordProvider.isProperNoun word
        
        Assert.False result

    [<Fact>]
    let detectsNoCzechNoun() = 
        let word = "koukat"

        let result = WordProvider.isProperNoun word
        
        Assert.False result

    [<Fact>]
    let detectsNoCzechDeclination() = 
        let word = "antilopu"

        let result = WordProvider.isProperNoun word
        
        Assert.False result

    [<Fact>]
    let detectsPlural() =
        let word = "panda"

        let result = WordProvider.hasPlural word

        Assert.True result

    [<Fact>]
    let detectsNoPlural() = 
        let word = "Oxford"

        let result = WordProvider.hasPlural word
        
        Assert.False result        