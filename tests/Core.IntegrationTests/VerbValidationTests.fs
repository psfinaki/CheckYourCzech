module VerbValidationTests

open Xunit
open VerbValidation

let getArticle = 
    Article.getArticle
    >> Option.get

[<Theory>]
[<InlineData "spát">]
[<InlineData "milovat">]
let ``Detects valid word for participles`` word =
    word
    |> getArticle
    |> parseVerbParticiple
    |> Option.isSome
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "nový">]
let ``Detects invalid word for participles`` word =
    word
    |> getArticle
    |> parseVerbParticiple
    |> Option.isSome
    |> Assert.False

[<Theory>]
[<InlineData "spát">]
[<InlineData "milovat">]
let ``Detects valid word for imperatives`` word =
    word
    |> getArticle
    |> parseVerbImperative
    |> Option.isSome
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "nový">]
let ``Detects invalid word for imperatives`` word =
    word
    |> getArticle
    |> parseVerbImperative
    |> Option.isSome
    |> Assert.False

[<Theory>]
[<InlineData "spát">]
[<InlineData "milovat">]
let ``Detects valid word for conjugations`` word =
    word
    |> getArticle
    |> parseVerbConjugation
    |> Option.isSome
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "nový">]
let ``Detects invalid word for conjugations`` word =
    word
    |> getArticle
    |> parseVerbConjugation
    |> Option.isSome
    |> Assert.False
