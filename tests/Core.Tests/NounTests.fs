module NounTests

open Xunit
open Noun
open Genders

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Fact>]
let ``Gets gender masculine animate``() =
    "tata"
    |> getGender
    |> equals MasculineAnimate

[<Fact>]
let ``Gets gender masculine inanimate``() =
    "hrad"
    |> getGender
    |> equals MasculineInanimate

[<Fact>]
let ``Gets gender feminine``() =
    "panda"
    |> getGender
    |> equals Feminine

[<Fact>]
let ``Gets gender neuter``() =
    "okno"
    |> getGender
    |> equals Neuter

[<Fact>]
let ``Gets declension wiki - indeclinable``() = 
    "dada"
    |> getDeclensionWiki Case.Nominative Number.Plural
    |> equals "dada"

[<Fact>]
let ``Gets declension wiki - editable article``() = 
    "panda"
    |> getDeclensionWiki Case.Nominative Number.Plural
    |> equals "pandy"

[<Fact>]
let ``Gets wiki plural wiki - locked article``() = 
    "debil"
    |> getDeclensionWiki Case.Nominative Number.Plural
    |> equals "debilové"

[<Fact>]
let ``Gets declension - indeclinable``() =
    "dada"
    |> getDeclension Case.Nominative Number.Singular
    |> equals [|"dada"|]

[<Fact>]
let ``Gets declension - single option``() =
    "hrad"
    |> getDeclension Case.Nominative Number.Singular
    |> equals [|"hrad"|]

[<Fact>]
let ``Gets declension - no options``() =
    "záda"
    |> getDeclension Case.Nominative Number.Singular
    |> equals [||]

[<Fact>]
let ``Gets singulars - multiple options``() =
    "temeno"
    |> getDeclension Case.Nominative Number.Singular
    |> equals [|"temeno"; "témě"|]
    
[<Fact>]
let ``Detects indeclinable``() =
    "dada"
    |> isIndeclinable
    |> Assert.True
    
[<Fact>]
let ``Detects declinable``() =
    "panda"
    |> isIndeclinable
    |> Assert.False
