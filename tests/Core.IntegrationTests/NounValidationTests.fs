module NounValidationTests

open Xunit
open NounValidation

let getArticle = 
    Article.getArticle
    >> Option.get

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
let ``Detects invalid word for accusatives`` word =
    word
    |> getArticle
    |> parseNounAccusative
    |> Option.isSome
    |> Assert.False
