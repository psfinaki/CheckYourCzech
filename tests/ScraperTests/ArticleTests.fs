module ArticleTests

open Xunit
open Article

let equals (x: string list) (y: seq<string>) = 
    Assert.Equal<string list>(x, Seq.toList y)

[<Fact>]
let getsName() =
    "panda"
    |> getName
    |> (=) "panda"
    |> Assert.True

[<Fact>]
let getsTableOfContents() =
    "panda"
    |> tryGetTableOfContents
    |> Option.isSome
    |> Assert.True

[<Fact>]
let detectsNoTableOfContents() =
    "alik"
    |> tryGetTableOfContents
    |> Option.isNone
    |> Assert.True
    
[<Fact>]
let detectsContent() =
    "panda"
    |> tryGetContent
    |> Option.isSome
    |> Assert.True

[<Fact>]
let getsPart() =
    "panda"
    |> getContent
    |> tryGetPart "čeština"
    |> Option.isSome
    |> Assert.True

[<Fact>]
let detectsNoPart() =
    "panda"
    |> getContent
    |> tryGetPart "ruština"
    |> Option.isNone
    |> Assert.True

[<Fact>]
let detectsInfo() = 
    "panda"
    |> getContent
    |> getPart "čeština"
    |> getInfo "rod"
    |> (=) "rod ženský"
    |> Assert.True

[<Fact>]
let detectsNoInfo() = 
    "panda"
    |> getContent
    |> tryGetInfo "evil"
    |> Option.isNone
    |> Assert.True

[<Fact>]
let getsPartNames() =
    "drak"
    |> getContent
    |> getPartNames
    |> equals [ "čeština"; "slovenština"; "poznámky"; "externí odkazy" ]
