module NounValidationTests

open Xunit
open NounValidation

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

[<Theory>]
[<InlineData "láska">]
[<InlineData "kachna">]
let ``Detects valid word for plurals`` word =
    word
    |> getArticle
    |> parseNounPlural
    |> Option.isSome
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "nový">]
[<InlineData "nůžky">]
let ``Detects invalid word for plurals`` word =
    word
    |> getArticle
    |> parseNounPlural
    |> Option.isSome
    |> Assert.False

[<Theory>]
[<InlineData "láska">]
[<InlineData "kachna">]
let ``Detects valid word for accusatives`` word =
    word
    |> getArticle
    |> parseNounAccusative
    |> Option.isSome
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "nový">]
[<InlineData "nůžky">]
let ``Detects invalid word for accusatives`` word =
    word
    |> getArticle
    |> parseNounAccusative
    |> Option.isSome
    |> Assert.False
