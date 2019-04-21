module VerbTests

open Xunit
open Verb

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Theory>]
[<InlineData("řídit", 4)>]
[<InlineData("řídit se", 4)>]
let ``Gets class`` verb ``class`` =
    verb
    |> getClass
    |> equals (Some ``class``)

[<Fact>]
let ``Detects imperative``() =
    "milovat"
    |> hasImperative
    |> Assert.True

[<Fact>]
let ``Detects no imperative - no conjugation``() =
    "chlastati"
    |> hasImperative
    |> Assert.False

[<Fact>]
let ``Detects no imperative - imperative does not exist``() =
    "moct"
    |> hasImperative
    |> Assert.False

[<Fact>]
let ``Gets imperatives - single option``() =
    "milovat"
    |> getImperatives
    |> equals [|"miluj"|]

[<Fact>]
let ``Gets imperatives - multiple options``() =
    "orat"
    |> getImperatives
    |> equals [|"oř"; "orej"|]
