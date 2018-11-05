module VerbTests

open Xunit
open Verb

let equals (x: string[]) (y: string[]) = Assert.Equal<string []>(x, y)

[<Fact>]
let ``Detects imperatives present``() = 
    "myslet"
    |> hasImperatives
    |> Assert.True

[<Fact>]
let ``Detects imperatives absent``() = 
    "musit"
    |> hasImperatives
    |> Assert.False

[<Fact>]
let ``Parses imperatives - one option``() = 
    "spi"
    |> parseImperatives
    |> equals [|"spi"|]

[<Fact>]
let ``Parses imperatives - multiple options``() = 
    "čisť / čisti"
    |> parseImperatives
    |> equals [|"čisť"; "čisti"|]

[<Fact>]
let ``Parses imperatives - bookish``() = 
    "prohraj / (knižně) prohrej"
    |> parseImperatives
    |> equals [|"prohraj"; "prohrej"|]

[<Fact>]
let ``Parses imperatives - informal``() = 
    "slzej, (hovorově) slz"
    |> parseImperatives
    |> equals [|"slzej"|]

[<Fact>]
let ``Parses imperatives - rearer``() = 
    "pohaň / (řidč.) pohaněj"
    |> parseImperatives
    |> equals [|"pohaň"; "pohaněj"|]

[<Fact>]
let ``Gets imperatives - no options``() = 
    "musit"
    |> getImperatives
    |> equals [||]

[<Fact>]
let ``Parses participles - one option``() = 
    "spal"
    |> parseParticiples
    |> equals [|"spal"|]

[<Fact>]
let ``Parses participles - multiple options``() = 
    "klepl / klepnul"
    |> parseParticiples
    |> equals [|"klepl"; "klepnul"|]
    
[<Fact>]
let ``Validates proper verb``() =
    "spát"
    |> isValid 
    |> Assert.True

[<Fact>]
let ``Invalidates improper verb - no Czech``() =
    "good"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper verb - no verb``() =
    "nazdar"
    |> isValid
    |> Assert.False

[<Fact>]
let ``Invalidates improper verb - no imperative``() =
    "chlastati"
    |> isValid
    |> Assert.False