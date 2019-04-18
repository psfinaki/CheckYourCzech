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
let ``Gets declension for case - indeclinable``() =
    "dada"
    |> getDeclensionForCase Case.Nominative Number.Singular
    |> equals [|"dada"|]

[<Fact>]
let ``Gets declension for case - single option``() =
    "hrad"
    |> getDeclensionForCase Case.Nominative Number.Singular
    |> equals [|"hrad"|]

[<Fact>]
let ``Gets declension for case - no options``() =
    "záda"
    |> getDeclensionForCase Case.Nominative Number.Singular
    |> equals [||]

[<Fact>]
let ``Gets singulars - multiple options``() =
    "temeno"
    |> getDeclensionForCase Case.Nominative Number.Singular
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

[<Fact>]
let ``Detects single declension for case``() =
    "panda"
    |> hasSingleDeclensionForCase Case.Nominative Number.Singular
    |> Assert.True

[<Fact>]
let ``Detects multiple declensions for case``() =
    "temeno"
    |> hasSingleDeclensionForCase Case.Nominative Number.Singular
    |> Assert.False

[<Fact>]
let ``Detects case declension is present``() =
    "panda"
    |> hasDeclensionForCase Case.Nominative Number.Singular
    |> Assert.True

[<Fact>]
let ``Detects no declension for case``() =
    "záda"
    |> hasDeclensionForCase Case.Nominative Number.Singular
    |> Assert.False

[<Fact>]
let ``Detects declension``() =
    "panda"
    |> hasDeclension
    |> Assert.True

[<Fact>]
let ``Detects no declension``() =
    "antilopu"
    |> hasDeclension
    |> Assert.False

[<Fact>]
let ``Detects gender``() =
    "panda"
    |> hasGender
    |> Assert.True

[<Fact>]
let ``Detects no gender - gender not specified``() =
    "trosečník"
    |> hasGender
    |> Assert.False

[<Fact>]
let ``Detects no gender - incorrect gender``() =
    "benediktin"
    |> hasGender
    |> Assert.False
