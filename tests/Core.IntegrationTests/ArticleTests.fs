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

[<Theory>]
[<InlineData "panda">]
[<InlineData "mnoho myslivců – zajícova smrt">]
[<InlineData "הַהֶרְגֵּל – טֶבַע שֵׁנִי">]
[<InlineData "lengyel, magyar – két jó barát, együtt harcol, s issza borát">]
[<InlineData "El cielo está enladrillado. ¿Quién lo desenladrillará? El desenladrillador que lo desenladrille, buen desenladrillador será.">]
let ``Gets name`` title =
    title
    |> getName
    |> equals title

[<Fact>]
let ``Gets table of contents``() =
    "panda"
    |> tryGetTableOfContents
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Detects content``() =
    "panda"
    |> tryGetContent
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Gets parts``() =
    "panda"
    |> getContent
    |> getParts "výslovnost"
    |> Seq.length
    |> equals 4

[<Fact>]
let ``Gets child part``() =
    "panda"
    |> getContent
    |> tryGetChildPart "čeština"
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Detects no child part``() =
    "panda"
    |> getContent
    |> tryGetChildPart "ruština"
    |> Option.isNone
    |> Assert.True

[<Fact>]
let ``Detects info``() = 
    "panda"
    |> getContent
    |> getChildPart "čeština"
    |> getInfo "rod"
    |> equals "rod ženský"

[<Fact>]
let ``Detects no info``() = 
    "panda"
    |> getContent
    |> tryGetInfo "evil"
    |> Option.isNone
    |> Assert.True

[<Fact>]
let ``Gets all parts``() =
    "spinkat"
    |> getContent
    |> getAllParts
    |> Seq.map fst
    |> equalsMany [ "čeština"; "výslovnost"; "dělení"; "sloveso"; "časování"; "význam"; "synonyma" ]

[<Fact>]
let ``Gets children parts``() =
    "spinkat"
    |> getContent
    |> getChildrenParts
    |> Seq.map fst
    |> equalsMany [ "čeština" ]

[<Fact>]
let ``Gets children parts - inner``() =
    "ananas"
    |> getContent
    |> getChildPart "čeština"
    |> getChildrenParts
    |> Seq.map fst
    |> equalsMany [ "výslovnost"; "dělení"; "podstatné jméno" ]

[<Fact>]
let ``Detects no parts``() =
    "provozovat"
    |> getContent
    |> getChildrenParts
    |> Seq.map fst
    |> equalsMany []

[<Fact>]
let ``Gets tables``() =
    "musit"
    |> getContent
    |> getChildPart "čeština"
    |> getChildPart "sloveso"
    |> getChildPart "časování"
    |> getTables
    |> Seq.map fst
    |> equalsMany [ "Oznamovací způsob"; "Příčestí"; "Přechodníky" ]

[<Fact>]
let ``Detects no tables``() =
    "musit"
    |> getContent
    |> getChildPart "čeština"
    |> getChildPart "sloveso"
    |> getChildPart "význam"
    |> getTables
    |> Seq.map fst
    |> equalsMany []

[<Fact>]
let ``Detects no parts - inner``() =
    "ananas"
    |> getContent
    |> getChildPart "čeština"
    |> getChildPart "výslovnost"
    |> getChildrenParts
    |> Seq.map fst
    |> equalsMany []

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
