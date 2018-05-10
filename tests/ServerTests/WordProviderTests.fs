namespace ServerTests

open ServerCode
open Xunit

module WordProviderTests =

    [<Fact>]
    let detectsCzechWord() =
        let word = "panda"

        let result = WordProvider.existsInCzech word

        Assert.True result

    [<Fact>]
    let detectsNotCzechWord() =
        let word = "table"

        let result = WordProvider.existsInCzech word

        Assert.False result

    [<Fact>]
    let detectsCzechNoun() = 
        let word = "hudba"

        let result = WordProvider.isCzechNoun word
        
        Assert.True result

    [<Fact>]
    let detectsCzechNotNoun() = 
        let word = "koukat"

        let result = WordProvider.isCzechNoun word
        
        Assert.False result
    