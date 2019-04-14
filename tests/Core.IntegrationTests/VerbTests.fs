module VerbTests

open System
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
