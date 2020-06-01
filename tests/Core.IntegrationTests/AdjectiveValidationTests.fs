module Core.IntegrationTests.AdjectiveValidationTests

open Xunit

open Core.Validation.AdjectiveValidation

[<Theory>]
[<InlineData "nový">]
[<InlineData "starý">]
let ``Detects valid word for plurals`` word =
    word
    |> getArticle 
    |> parseAdjectivePlural
    |> Option.isSome
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "láska">]
let ``Detects invalid word for plurals`` word =
    word
    |> getArticle 
    |> parseAdjectivePlural
    |> Option.isSome
    |> Assert.False

[<Theory>]
[<InlineData "nový">]
[<InlineData "starý">]
let ``Detects valid word for comparatives`` word =
    word
    |> getArticle 
    |> parseAdjectiveComparative
    |> Option.isSome
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "láska">]
let ``Detects invalid word for comparatives`` word =
    word
    |> getArticle 
    |> parseAdjectiveComparative
    |> Option.isSome
    |> Assert.False
