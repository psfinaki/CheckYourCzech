namespace ServerTests

open Xunit
open AnswerProvider

module AnswerProviderTests =

    let equals (x: string[]) (y: string[]) = Assert.Equal<string []>(x, y)

    [<Fact>]
    let getsPlural() = 
        "svetr"
        |> getPlural
        |> equals [|"svetry"|]

    [<Fact>]
    let getsPluralSeveralOptions() = 
        "medvěd"
        |> getPlural
        |> equals [|"medvědi"; "medvědové"|]

    [<Fact>]
    let getsPluralSeveralOptionsNoSpaces() = 
        "Edáček"
        |> getPlural
        |> equals [|"Edáčci"; "Edáčkové"|]

    [<Fact>]
    let getsPluralNoOptions() = 
        "Oxford"
        |> getPlural
        |> equals [||] 
