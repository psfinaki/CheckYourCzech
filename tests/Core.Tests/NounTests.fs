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
let ``Gets wiki plural - indeclinable``() = 
    "dada"
    |> getWikiPlural
    |> equals "dada"

[<Fact>]
let ``Gets wiki plural - common article``() = 
    "panda"
    |> getWikiPlural
    |> equals "pandy"

[<Fact>]
let ``Gets wiki plural - locked article``() = 
    "debil"
    |> getWikiPlural
    |> equals "debilové"

[<Fact>]
let ``Gets wiki accusative - indeclinable``() = 
    "dada"
    |> getWikiAccusative
    |> equals "dada"

[<Fact>]
let ``Gets wiki accusative - common article``() = 
    "panda"
    |> getWikiAccusative
    |> equals "pandu"

[<Fact>]
let ``Gets wiki accusative - locked article``() = 
    "debil"
    |> getWikiAccusative
    |> equals "debila"

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
    
[<Fact>]
let ``Gets singulars - indeclinable``() =
    "dada"
    |> getSingulars
    |> equals [|"dada"|]

[<Fact>]
let ``Gets singulars - single option``() =
    "hrad"
    |> getSingulars
    |> equals [|"hrad"|]

[<Fact>]
let ``Gets singulars - no options``() =
    "záda"
    |> getSingulars
    |> equals [||]

[<Fact>]
let ``Gets singulars - multiple options``() =
    "temeno"
    |> getSingulars
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
