module VerbValidationTests

open Xunit
open VerbValidation

[<Theory>]
[<InlineData "spát">]
[<InlineData "milovat">]
let ``Detects valid word for participles`` word =
    word
    |> isParticipleValid
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "nový">]
let ``Detects invalid word for participles`` word =
    word
    |> isParticipleValid
    |> Assert.False

[<Theory>]
[<InlineData "spát">]
[<InlineData "milovat">]
let ``Detects valid word for imperatives`` word =
    word
    |> isImperativeValid
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "nový">]
let ``Detects invalid word for imperatives`` word =
    word
    |> isImperativeValid
    |> Assert.False
