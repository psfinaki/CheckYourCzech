module Core.IntegrationTests.NounValidationTests

open Xunit

open Core.Validation.NounValidation

[<Theory>]
[<InlineData "láska">]
[<InlineData "kachna">]
[<InlineData "nůžky">]
let ``Detects valid word for declension`` word =
    word
    |> getArticle
    |> parseNoun
    |> Option.isSome
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "nový">]
[<InlineData "vrchní">]
let ``Detects invalid word for declension`` word =
    word
    |> getArticle
    |> parseNoun
    |> Option.isSome
    |> Assert.False
