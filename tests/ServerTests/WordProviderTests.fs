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
    let detectsCzechNotNoun() = 
        let word = "koukat"

        let result = WordProvider.isAppropriate word
        
        Assert.False result
    