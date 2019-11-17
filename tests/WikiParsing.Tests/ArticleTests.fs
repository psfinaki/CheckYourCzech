module ArticleTests

open Xunit
open Article

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
    |> getPart "čeština"
    |> getParts
    |> Seq.map fst
    |> seqEquals [ "výslovnost"; "dělení"; "podstatné jméno" ]

[<Fact>]
let ``Detects no children parts``() =
    "provozovat"
    |> getContent
    |> getParts
    |> Seq.map fst
    |> seqEquals []

[<Fact>]
let ``Detects child part``() =
    "panda"
    |> getContent
    |> hasPart "čeština"
    |> Assert.True

[<Fact>]
let ``Detects no child part``() =
    "panda"
    |> getContent
    |> hasPart "ruština"
    |> Assert.False

[<Fact>]
let ``Detects children parts - filter``() =
    "panda"
    |> getContent
    |> hasPartsWhen ((=) "čeština")
    |> Assert.True

[<Fact>]
let ``Detects no children parts - filter``() =
    "panda"
    |> getContent
    |> hasPartsWhen ((=) "ruština")
    |> Assert.False

[<Fact>]
let ``Gets infos``() = 
    "panda"
    |> getContent
    |> getPart "čeština"
    |> getInfos (Starts "rod")
    |> seqEquals ["rod ženský"]

[<Fact>]
let ``Detects info``() = 
    "mozek"
    |> getContent
    |> hasInfo (Is "chytrý")
    |> Assert.True

[<Fact>]
let ``Detects no info``() = 
    "panda"
    |> getContent
    |> hasInfo (Is "evil")
    |> Assert.False

[<Fact>]
let ``Gets tables``() =
    "musit"
    |> getContent
    |> getPart "čeština"
    |> getPart "sloveso"
    |> getPart "časování"
    |> getTables
    |> Seq.map fst
    |> seqEquals [ "Oznamovací způsob"; "Příčestí"; "Přechodníky" ]

[<Fact>]
let ``Detects no tables``() =
    "musit"
    |> getContent
    |> getPart "čeština"
    |> getPart "sloveso"
    |> getPart "význam"
    |> getTables
    |> Seq.map fst
    |> seqEquals []

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
    |> seqEquals [ "podstatné jméno"; "přídavné jméno" ]

[<Fact>]
let ``Detects no parts of speech``() =
    "hello"
    |> getPartsOfSpeech
    |> seqEquals []

[<Fact>]
let ``Gets match``() =
    "panda"
    |> ``match`` [
        Is "čeština"
        Is "podstatné jméno"
        Is "skloňování"
    ]
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Gets no match``() =
    "panda"
    |> ``match`` [
        Is "čeština"
        Is "přídavné jméno"
        Is "skloňování"
    ]
    |> equals Option.None

[<Fact>]
let ``Gets matches``() =
    "bus"
    |> matches [
        Is "čeština"
        Starts "podstatné jméno"
    ]
    |> Seq.length
    |> equals 2

[<Fact>]
let ``Gets no matches``() =
    "panda"
    |> matches [
        Is "čeština"
        Is "přídavné jméno"
        Is "skloňování"
    ]
    |> Assert.Empty

[<Fact>]
let ``Detects match``() =
    "čtvrt"
    |> isMatch [
        Is "čeština"
        Is "podstatné jméno"
        Starts "skloňování"
    ]
    |> Assert.True

[<Fact>]
let ``Detects no match``() =
    "bus"
    |> isMatch [
        Is "čeština"
        Is "přídavné jméno"
        Is "skloňování"
    ]
    |> Assert.False
