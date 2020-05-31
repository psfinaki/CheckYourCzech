module Core.Tests.ComparisonTests

open Xunit

open Core.Adjectives.Comparison

[<Fact>]
let ``Detects syntactic comparison``() = 
    "více hyzdící"
    |> isSyntacticComparison
    |> Assert.True

[<Fact>]
let ``Detects morphological comparison``() = 
    "novější"
    |> isSyntacticComparison
    |> Assert.False

[<Theory>]
[<InlineData "dobrý">]
[<InlineData "dobrá">]
[<InlineData "dobré">]
let ``Detects hard positive`` adjective = 
    adjective
    |> isHardPositive
    |> Assert.True

[<Theory>]
[<InlineData "lepší">]
[<InlineData "moderní">]
let ``Detects not hard positive`` adjective = 
    adjective
    |> isHardPositive
    |> Assert.False

[<Fact>]
let ``Detects soft positive``() = 
    "moderní"
    |> isSoftPositive
    |> Assert.True

[<Theory>]
[<InlineData "černý">]
[<InlineData "modernější">]
let ``Detects not soft positive`` adjective = 
    adjective
    |> isSoftPositive
    |> Assert.False
