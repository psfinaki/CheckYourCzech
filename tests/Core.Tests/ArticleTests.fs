module ArticleTests

open Xunit
open Article

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)
let equalsMany (x: string list) (y: seq<string>) = Assert.Equal<string list>(x, Seq.toList y)

[<Fact>]
let ``Detects no table of contents``() =
    "alik"
    |> tryGetTableOfContents
    |> Option.isNone
    |> Assert.True
