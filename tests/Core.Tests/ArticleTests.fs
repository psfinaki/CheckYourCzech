module ArticleTests

open Xunit
open Article

let equals (x: string list) (y: seq<string>) = Assert.Equal<string list>(x, Seq.toList y)

[<Fact>]
let ``Gets name``() =
    "panda"
    |> getName
    |> (=) "panda"
    |> Assert.True

[<Fact>]
let ``Gets table of contents``() =
    "panda"
    |> tryGetTableOfContents
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Detects no table of contents``() =
    "alik"
    |> tryGetTableOfContents
    |> Option.isNone
    |> Assert.True
    
[<Fact>]
let ``Detects content``() =
    "panda"
    |> tryGetContent
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Gets part``() =
    "panda"
    |> getContent
    |> tryGetPart "čeština"
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Detects no part``() =
    "panda"
    |> getContent
    |> tryGetPart "ruština"
    |> Option.isNone
    |> Assert.True

[<Fact>]
let ``Detects info``() = 
    "panda"
    |> getContent
    |> getPart "čeština"
    |> getInfo "rod"
    |> (=) "rod ženský"
    |> Assert.True

[<Fact>]
let ``Detects no info``() = 
    "panda"
    |> getContent
    |> tryGetInfo "evil"
    |> Option.isNone
    |> Assert.True

[<Fact>]
let ``Gets part names``() =
    "drak"
    |> getContent
    |> getPartNames
    |> equals [ "čeština"; "slovenština"; "poznámky"; "externí odkazy" ]

[<Fact>]
let ``Detects no part names``() =
    "provozovat"
    |> getContent
    |> getPartNames
    |> equals []