module AnswerProviderTests

open Xunit
open AnswerProvider
open Gender

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

[<Fact>]
let getsGenderMasculineAnimate() =
    "tata"
    |> getGender
    |> (=) MasculineAnimate
    |> Assert.True

[<Fact>]
let getsGenderMasculineInanimate() =
    "hrad"
    |> getGender
    |> (=) MasculineInanimate
    |> Assert.True

[<Fact>]
let getsGenderFeminine() =
    "panda"
    |> getGender
    |> (=) Feminine
    |> Assert.True

[<Fact>]
let getsGenderNeuter() =
    "okno"
    |> getGender
    |> (=) Neuter
    |> Assert.True
