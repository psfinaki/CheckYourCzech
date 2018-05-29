namespace ServerTests

open ServerCode
open Xunit

module AnswerProviderTests =

    [<Fact>]
    let getsPlural() = 
        let word = "svetr"
        let expected = [|"svetry"|]

        let actual = AnswerProvider.getPlural word
        
        Assert.Equal<string []>(expected, actual)

    [<Fact>]
    let getsPluralSeveralOptions() = 
        let word = "medvěd"
        let expected = [|"medvědi"; "medvědové"|]

        let actual = AnswerProvider.getPlural word
        
        Assert.Equal<string []>(expected, actual)

    [<Fact>]
    let getsPluralSeveralOptionsNoSpaces() = 
        let word = "Edáček"
        let expected = [|"Edáčci"; "Edáčkové"|]

        let actual = AnswerProvider.getPlural word
        
        Assert.Equal<string []>(expected, actual)

    [<Fact>]
    let getsPluralNoOptions() = 
        let word = "Oxford"
        let expected = [||] 

        let actual = AnswerProvider.getPlural word
        
        Assert.Equal<string []>(expected, actual)