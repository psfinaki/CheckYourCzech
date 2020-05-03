module ArticleTests

open Xunit
open Article

[<Fact>]
let ``Detects content``() =
    "panda"
    |> Helper.getArticle

[<Fact>]
let ``Gets children parts``() =
    "ananas"
    |> Helper.getArticle
    |> getContent
    |> getPart "čeština"
    |> Option.map getParts
    |> Option.map (Seq.map fst >> Seq.toList)
    |> equals (Some [ "výslovnost"; "dělení"; "podstatné jméno" ])

[<Fact>]
let ``Detects no children parts``() =
    "ananas"
    |> Helper.getArticle
    |> getContent
    |> getPart "poznámky"
    |> Option.map getParts
    |> Option.map (Seq.map fst)
    |> Option.contains Seq.empty

[<Fact>]
let ``Detects child part``() =
    "panda"
    |> Helper.getArticle
    |> getContent
    |> getPart "čeština"
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Detects no child part``() =
    "panda"
    |> Helper.getArticle
    |> getContent
    |> getPart "ruština"
    |> Option.isSome
    |> Assert.False

[<Fact>]
let ``Detects children parts - filter``() =
    "panda"
    |> Helper.getArticle
    |> getContent
    |> hasPartsWhen ((=) "čeština")
    |> Assert.True

[<Fact>]
let ``Detects no children parts - filter``() =
    "panda"
    |> Helper.getArticle
    |> getContent
    |> hasPartsWhen ((=) "ruština")
    |> Assert.False

[<Fact>]
let ``Gets infos``() = 
    "panda"
    |> Helper.getArticle
    |> getContent
    |> getPart "čeština"
    |> Option.map (getInfos (Starts "rod") >> Seq.toList)
    |> equals (Some ["rod ženský"])

[<Fact>]
let ``Detects info``() = 
    "mozek"
    |> Helper.getArticle
    |> getContent
    |> hasInfo (Is "chytrý")
    |> Assert.True

[<Fact>]
let ``Detects no info``() = 
    "panda"
    |> Helper.getArticle
    |> getContent
    |> hasInfo (Is "evil")
    |> Assert.False

[<Fact>]
let ``Gets tables``() =
    "musit"
    |> Helper.getArticle
    |> getContent
    |> getPart "čeština"
    |> Option.bind (getPart "sloveso")
    |> Option.bind (getPart "časování")
    |> Option.map getTables
    |> Option.map (Seq.map fst >> Seq.toList)
    |> equals (Some [ "Oznamovací způsob"; "Příčestí"; "Přechodníky" ])

[<Fact>]
let ``Detects no tables``() =
    "musit"
    |> Helper.getArticle
    |> getContent
    |> getPart "čeština"
    |> Option.bind (getPart "sloveso")
    |> Option.bind (getPart "význam")
    |> Option.map getTables
    |> Option.map (Seq.map fst >> Seq.toList)
    |> equals (Some [])

[<Fact>]
let ``Detects non-locked article``() =
    "hudba"
    |> Helper.getArticle 
    |> isLocked
    |> Assert.False

[<Fact>]
let ``Detects locked article``() =
    "debil"
    |> Helper.getArticle 
    |> isLocked
    |> Assert.True

[<Fact>]
let ``Gets parts of speech``() =
    "starý"
    |> Helper.getArticle 
    |> getPartsOfSpeech
    |> seqEquals [ "podstatné jméno"; "přídavné jméno" ]

[<Fact>]
let ``Detects no parts of speech``() =
    "hello"
    |> Helper.getArticle
    |> getPartsOfSpeech
    |> seqEquals []

[<Fact>]
let ``Gets match``() =
    "panda"
    |> Helper.getArticle
    |> ``match`` [
        Is "podstatné jméno"
        Is "skloňování"
    ]
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Gets no match``() =
    "panda"
    |> Helper.getArticle
    |> ``match`` [
        Is "přídavné jméno"
        Is "skloňování"
    ]
    |> equals Option.None

[<Fact>]
let ``Gets matches``() =
    "bus"
    |> Helper.getArticle
    |> matches [
        Starts "podstatné jméno"
    ]
    |> Seq.length
    |> equals 2

[<Fact>]
let ``Gets no matches``() =
    "panda"
    |> Helper.getArticle
    |> matches [
        Is "přídavné jméno"
        Is "skloňování"
    ]
    |> Assert.Empty

[<Fact>]
let ``Detects match``() =
    "čtvrt"
    |> Helper.getArticle
    |> isMatch [
        Is "podstatné jméno"
        Starts "skloňování"
    ]
    |> Assert.True

[<Fact>]
let ``Detects no match``() =
    "bus"
    |> Helper.getArticle
    |> isMatch [
        Is "přídavné jméno"
        Is "skloňování"
    ]
    |> Assert.False
