module AdjectiveValidationTests

open Xunit
open AdjectiveValidation

[<Theory>]
[<InlineData "nový">]
[<InlineData "starý">]
let ``Detects valid word for plurals`` word =
    word
    |> isPluralValid
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "láska">]
let ``Detects invalid word for plurals`` word =
    word
    |> isPluralValid
    |> Assert.False

[<Theory>]
[<InlineData "nový">]
[<InlineData "starý">]
let ``Detects valid word for comparatives`` word =
    word
    |> isComparativeValid
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "láska">]
let ``Detects invalid word for comparatives`` word =
    word
    |> isComparativeValid
    |> Assert.False
