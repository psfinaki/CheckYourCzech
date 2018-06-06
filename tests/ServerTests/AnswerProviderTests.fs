namespace ServerTests

open Xunit

module AnswerProviderTests =

    let equals (x: string[]) (y: string[]) = Assert.Equal<string []>(x, y)

    [<Fact>]
    let getsPlural() = 
        "svetr"
        |> AnswerProvider.getPlural
        |> equals [|"svetry"|]

    [<Fact>]
    let getsPluralSeveralOptions() = 
        "medvěd"
        |> AnswerProvider.getPlural
        |> equals [|"medvědi"; "medvědové"|]

    [<Fact>]
    let getsPluralSeveralOptionsNoSpaces() = 
        "Edáček"
        |> AnswerProvider.getPlural
        |> equals [|"Edáčci"; "Edáčkové"|]

    [<Fact>]
    let getsPluralNoOptions() = 
        "Oxford"
        |> AnswerProvider.getPlural
        |> equals [||] 
