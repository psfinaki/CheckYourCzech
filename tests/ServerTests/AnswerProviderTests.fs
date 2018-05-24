namespace ServerTests

open ServerCode
open Xunit

module AnswerProviderTests =

    [<Fact>]
    let getsMultiple() = 
        let word = "svetr"
        let expected = [|"svetry"|]

        let actual = AnswerProvider.getMultiple word
        
        Assert.Equal<string[]> (expected, actual)

    [<Fact>]
    let getsMultipleSeveralOptions() = 
        let word = "medvěd"
        let expected = [|"medvědi"; "medvědové"|]

        let actual = AnswerProvider.getMultiple word
        
        Assert.Equal<string[]> (expected, actual)