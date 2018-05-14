namespace ServerTests

open ServerCode
open Xunit

module WordProviderTests =

    [<Fact>]
    let detectsCzechNoun() = 
        let word = "hudba"

        let result = WordProvider.isAppropriate word
        
        Assert.True result

    [<Fact>]
    let detectsNoCzechPart() = 
        let word = "immigration"

        let result = WordProvider.isAppropriate word
        
        Assert.False result

    [<Fact>]
    let detectsNoCzechNoun() = 
        let word = "koukat"

        let result = WordProvider.isAppropriate word
        
        Assert.False result

    [<Fact>]
    let detectsNoCzechDeclination() = 
        let word = "antilopu"

        let result = WordProvider.isAppropriate word
        
        Assert.False result
        