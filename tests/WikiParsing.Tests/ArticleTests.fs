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
    |> getArticleName
    |> equals title

[<Fact>]
let ``Detects content``() =
    "panda"
    |> tryGetContent
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Gets children parts``() =
    "ananas"
    |> getContent
    |> getChildPart "čeština"
    |> getChildrenParts
    |> Seq.map fst
    |> equalsMany [ "výslovnost"; "dělení"; "podstatné jméno" ]

[<Fact>]
let ``Detects no children parts``() =
    "provozovat"
    |> getContent
    |> getChildrenParts
    |> Seq.map fst
    |> equalsMany []

[<Fact>]
let ``Detects child part``() =
    "panda"
    |> getContent
    |> hasChildPart "čeština"
    |> Assert.True

[<Fact>]
let ``Detects no child part``() =
    "panda"
    |> getContent
    |> hasChildPart "ruština"
    |> Assert.False

[<Fact>]
let ``Detects children parts - filter``() =
    "panda"
    |> getContent
    |> hasChildrenPartsWhen ((=) "čeština")
    |> Assert.True

[<Fact>]
let ``Detects no children parts - filter``() =
    "panda"
    |> getContent
    |> hasChildrenPartsWhen ((=) "ruština")
    |> Assert.False

[<Fact>]
let ``Gets info``() = 
    "panda"
    |> getContent
    |> getChildPart "čeština"
    |> getInfo "rod"
    |> equals "rod ženský"

[<Fact>]
let ``Detects info``() = 
    "mozek"
    |> getContent
    |> hasInfo "chytrý"
    |> Assert.True

[<Fact>]
let ``Detects no info``() = 
    "panda"
    |> getContent
    |> hasInfo "evil"
    |> Assert.False

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
let ``Detects non-locked article``() =
    "hudba"
    |> isLocked
    |> Assert.False

[<Fact>]
let ``Detects locked article``() =
    "debil"
    |> isLocked
    |> Assert.True

[<Fact>]
let ``Gets parts of speech``() =
    "starý"
    |> getPartsOfSpeech
    |> equalsMany [ "podstatné jméno"; "přídavné jméno" ]

[<Fact>]
let ``Detects no parts of speech``() =
    "hello"
    |> getPartsOfSpeech
    |> equalsMany []
