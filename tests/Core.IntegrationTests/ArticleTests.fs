module ArticleTests

open Xunit
open Article

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)
let equalsMany (x: string list) (y: seq<string>) = Assert.Equal<string list>(x, Seq.toList y)

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
    |> equals "rod ženský"

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
    |> equalsMany [ "čeština"; "slovenština"; "poznámky"; "externí odkazy" ]

[<Fact>]
let ``Gets parts - inner``() =
    "ananas"
    |> getContent
    |> getPart "čeština"
    |> getParts
    |> Seq.map fst
    |> equalsMany [ "výslovnost"; "dělení"; "podstatné jméno" ]

[<Fact>]
let ``Detects no parts``() =
    "provozovat"
    |> getContent
    |> getParts
    |> Seq.map fst
    |> equalsMany []

[<Fact>]
let ``Gets tables``() =
    "musit"
    |> getContent
    |> getPart "čeština"
    |> getPart "sloveso"
    |> getPart "časování"
    |> getTables
    |> Seq.map fst
    |> equalsMany [ "Oznamovací způsob"; "Příčestí"; "Přechodníky" ]

[<Fact>]
let ``Detects no tables``() =
    "musit"
    |> getContent
    |> getPart "čeština"
    |> getPart "sloveso"
    |> getPart "význam"
    |> getTables
    |> Seq.map fst
    |> equalsMany []

[<Fact>]
let ``Detects no parts - inner``() =
    "ananas"
    |> getContent
    |> getPart "čeština"
    |> getPart "výslovnost"
    |> getParts
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
