module NounTests

open Xunit
open Noun
open Gender

let equals (x: string[]) (y: string[]) = Assert.Equal<string []>(x, y)

[<Fact>]
let ``Gets gender masculine animate``() =
    "tata"
    |> getGender
    |> (=) MasculineAnimate
    |> Assert.True

[<Fact>]
let ``Gets gender masculine inanimate``() =
    "hrad"
    |> getGender
    |> (=) MasculineInanimate
    |> Assert.True

[<Fact>]
let ``Gets gender feminine``() =
    "panda"
    |> getGender
    |> (=) Feminine
    |> Assert.True

[<Fact>]
let ``Gets gender neuter``() =
    "okno"
    |> getGender
    |> (=) Neuter
    |> Assert.True

[<Fact>]
let ``Gets plural - one option``() = 
    "svetr"
    |> getPlurals
    |> equals [|"svetry"|]

[<Fact>]
let ``Gets plurals - multiple options, spaces near slash``() = 
    "medvěd"
    |> getPlurals
    |> equals [|"medvědi"; "medvědové"|]

[<Fact>]
let ``Gets plurals - multiple options, no spaces near slash``() = 
    "Edáček"
    |> getPlurals
    |> equals [|"Edáčci"; "Edáčkové"|]

[<Fact>]
let ``Gets plurals - no options``() = 
    "Oxford"
    |> getPlurals
    |> equals [||]

[<Fact>]
let ``Gets plurals - locked article``() = 
    "debil"
    |> getPlurals
    |> equals [|"debilové"|]

[<Fact>]
let ``Validates proper noun``() =
    "panda"
    |> isValid 
    |> Assert.True

[<Fact>]
let ``Invalidates improper noun - no Czech``() =
    "good"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper noun - no noun``() =
    "koukat"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper noun - no declension``() =
    "antilopu"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper noun - no gender``() =
    "trosečník"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper noun - incorrect gender``() =
    "benediktin"
    |> isValid
    |> Assert.False