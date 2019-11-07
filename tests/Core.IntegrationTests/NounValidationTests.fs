module NounValidationTests

open Xunit
open NounValidation

[<Theory>]
[<InlineData "láska">]
[<InlineData "krtek">]
let ``Detects valid word for plurals`` word =
    word
    |> isPluralValid
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "nový">]
let ``Detects invalid word for plurals`` word =
    word
    |> isPluralValid
    |> Assert.False

[<Theory>]
[<InlineData "láska">]
[<InlineData "krtek">]
let ``Detects valid word for accusatives`` word =
    word
    |> isAccusativeValid
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "nový">]
let ``Detects invalid word for accusatives`` word =
    word
    |> isAccusativeValid
    |> Assert.False
