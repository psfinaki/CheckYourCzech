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
let ``Gets parts``() =
    "drak"
    |> getContent
    |> getParts
    |> Seq.map fst
    |> equals [ "čeština"; "slovenština"; "poznámky"; "externí odkazy" ]

[<Fact>]
let ``Gets parts - inner``() =
    "ananas"
    |> getContent
    |> getPart "čeština"
    |> getParts
    |> Seq.map fst
    |> equals [ "výslovnost"; "dělení"; "podstatné jméno" ]

[<Fact>]
let ``Detects no parts``() =
    "provozovat"
    |> getContent
    |> getParts
    |> Seq.map fst
    |> equals []

[<Fact>]
let ``Detects no parts - inner``() =
    "ananas"
    |> getContent
    |> getPart "čeština"
    |> getPart "výslovnost"
    |> getParts
    |> Seq.map fst
    |> equals []

[<Fact>]
let ``Detects non-locked article``() =
    "hudba"
    |> isLocked
    |> Assert.False

[<Fact>]
let ``Detects locked article``() =
    "debil"
    |> isLocked
    |> Assert.True
