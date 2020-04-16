module ArticleTests

open Xunit
open Article

[<Fact>]
let ``Detects content``() =
    "panda"
    |> getArticle
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Gets children parts``() =
    "ananas"
    |> getArticle
    |> Option.get
    |> getContent
    |> getPart "čeština"
    |> Option.map getParts
    |> Option.map (Seq.map fst >> Seq.toList)
    |> equals (Some [ "výslovnost"; "dělení"; "podstatné jméno" ])

[<Fact>]
let ``Detects no children parts``() =
    "ananas"
    |> getArticle
    |> Option.get
    |> getContent
    |> getPart "poznámky"
    |> Option.map getParts
    |> Option.map (Seq.map fst)
    |> Option.contains Seq.empty

[<Fact>]
let ``Detects child part``() =
    "panda"
    |> getArticle
    |> Option.get
    |> getContent
    |> getPart "čeština"
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Detects no child part``() =
    "panda"
    |> getArticle
    |> Option.get
    |> getContent
    |> getPart "ruština"
    |> Option.isSome
    |> Assert.False

[<Fact>]
let ``Detects children parts - filter``() =
    "panda"
    |> getArticle
    |> Option.get
    |> getContent
    |> hasPartsWhen ((=) "čeština")
    |> Assert.True

[<Fact>]
let ``Detects no children parts - filter``() =
    "panda"
    |> getArticle
    |> Option.get
    |> getContent
    |> hasPartsWhen ((=) "ruština")
    |> Assert.False

[<Fact>]
let ``Gets infos``() = 
    "panda"
    |> getArticle
    |> Option.get
    |> getContent
    |> getPart "čeština"
    |> Option.map (getInfos (Starts "rod") >> Seq.toList)
    |> equals (Some ["rod ženský"])

[<Fact>]
let ``Detects info``() = 
    "mozek"
    |> getArticle
    |> Option.get
    |> getContent
    |> hasInfo (Is "chytrý")
    |> Assert.True

[<Fact>]
let ``Detects no info``() = 
    "panda"
    |> getArticle
    |> Option.get
    |> getContent
    |> hasInfo (Is "evil")
    |> Assert.False

[<Fact>]
let ``Gets tables``() =
    "musit"
    |> getArticle
    |> Option.get
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
    |> getArticle
    |> Option.get
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
    |> Article.getArticle 
    |> Option.get
    |> isLocked
    |> Assert.False

[<Fact>]
let ``Detects locked article``() =
    "debil"
    |> Article.getArticle 
    |> Option.get
    |> isLocked
    |> Assert.True

[<Fact>]
let ``Gets parts of speech``() =
    "starý"
    |> Article.getArticle 
    |> Option.get
    |> getPartsOfSpeech
    |> seqEquals [ "podstatné jméno"; "přídavné jméno" ]

[<Fact>]
let ``Detects no parts of speech``() =
    "hello"
    |> Article.getArticle
    |> Option.get
    |> getPartsOfSpeech
    |> seqEquals []

[<Fact>]
let ``Gets match``() =
    "panda"
    |> Article.getArticle
    |> Option.get
    |> ``match`` [
        Is "podstatné jméno"
        Is "skloňování"
    ]
    |> Option.isSome
    |> Assert.True

[<Fact>]
let ``Gets no match``() =
    "panda"
    |> Article.getArticle
    |> Option.get
    |> ``match`` [
        Is "přídavné jméno"
        Is "skloňování"
    ]
    |> equals Option.None

[<Fact>]
let ``Gets matches``() =
    "bus"
    |> Article.getArticle
    |> Option.get
    |> matches [
        Starts "podstatné jméno"
    ]
    |> Seq.length
    |> equals 2

[<Fact>]
let ``Gets no matches``() =
    "panda"
    |> Article.getArticle
    |> Option.get
    |> matches [
        Is "přídavné jméno"
        Is "skloňování"
    ]
    |> Assert.Empty

[<Fact>]
let ``Detects match``() =
    "čtvrt"
    |> Article.getArticle
    |> Option.get
    |> isMatch [
        Is "podstatné jméno"
        Starts "skloňování"
    ]
    |> Assert.True

[<Fact>]
let ``Detects no match``() =
    "bus"
    |> Article.getArticle
    |> Option.get
    |> isMatch [
        Is "přídavné jméno"
        Is "skloňování"
    ]
    |> Assert.False
