module ArticleTests

open Xunit
open Article

[<Fact>]
let ``Detects content``() =
    "panda"
    |> getContent
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Gets children parts``() =
    "ananas"
    |> getContent
    |> Option.bind (getPart "čeština")
    |> Option.map getParts
    |> Option.map (Seq.map fst >> Seq.toList)
    |> equals (Some [ "výslovnost"; "dělení"; "podstatné jméno" ])

[<Fact>]
let ``Detects no children parts``() =
    "provozovat"
    |> getContent
    |> Option.map getParts
    |> Option.map (Seq.map fst)
    |> Option.contains Seq.empty

[<Fact>]
let ``Detects child part``() =
    "panda"
    |> getContent
    |> Option.bind (getPart "čeština")
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Detects no child part``() =
    "panda"
    |> getContent
    |> Option.bind (getPart "ruština")
    |> Option.isSome
    |> Assert.False

[<Fact>]
let ``Detects children parts - filter``() =
    "panda"
    |> getContent
    |> Option.exists (hasPartsWhen ((=) "čeština"))
    |> Assert.True

[<Fact>]
let ``Detects no children parts - filter``() =
    "panda"
    |> getContent
    |> Option.exists (hasPartsWhen ((=) "ruština"))
    |> Assert.False

[<Fact>]
let ``Gets infos``() = 
    "panda"
    |> getContent
    |> Option.bind (getPart "čeština")
    |> Option.map (getInfos (Starts "rod") >> Seq.toList)
    |> equals (Some ["rod ženský"])

[<Fact>]
let ``Detects info``() = 
    "mozek"
    |> getContent
    |> Option.exists (hasInfo (Is "chytrý"))
    |> Assert.True

[<Fact>]
let ``Detects no info``() = 
    "panda"
    |> getContent
    |> Option.exists (hasInfo (Is "evil"))
    |> Assert.False

[<Fact>]
let ``Gets tables``() =
    "musit"
    |> getContent
    |> Option.bind (getPart "čeština")
    |> Option.bind (getPart "sloveso")
    |> Option.bind (getPart "časování")
    |> Option.map getTables
    |> Option.map (Seq.map fst >> Seq.toList)
    |> equals (Some [ "Oznamovací způsob"; "Příčestí"; "Přechodníky" ])

[<Fact>]
let ``Detects no tables``() =
    "musit"
    |> getContent
    |> Option.bind (getPart "čeština")
    |> Option.bind (getPart "sloveso")
    |> Option.bind (getPart "význam")
    |> Option.map getTables
    |> Option.map (Seq.map fst >> Seq.toList)
    |> equals (Some [])

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
        Is "podstatné jméno"
        Is "skloňování"
    ]
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Gets no match``() =
    "panda"
    |> ``match`` [
        Is "přídavné jméno"
        Is "skloňování"
    ]
    |> equals Option.None

[<Fact>]
let ``Gets matches``() =
    "bus"
    |> matches [
        Starts "podstatné jméno"
    ]
    |> Seq.length
    |> equals 2

[<Fact>]
let ``Gets no matches``() =
    "panda"
    |> matches [
        Is "přídavné jméno"
        Is "skloňování"
    ]
    |> Assert.Empty

[<Fact>]
let ``Detects match``() =
    "čtvrt"
    |> isMatch [
        Is "podstatné jméno"
        Starts "skloňování"
    ]
    |> Assert.True

[<Fact>]
let ``Detects no match``() =
    "bus"
    |> isMatch [
        Is "přídavné jméno"
        Is "skloňování"
    ]
    |> Assert.False
