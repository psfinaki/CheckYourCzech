module WikiParsing.Tests.VerbArticleTests

open Xunit

open WikiParsing.Articles.VerbArticle
open Common.Conjugation
open Common.WikiArticles

let getArticle =
    getArticle
    >> VerbArticle

[<Fact>]
let ``Gets imperatives - single option``() =
    "milovat"
    |> getArticle 
    |> VerbArticleWithImperative
    |> getImperatives
    |> seqEquals ["miluj"]

[<Fact>]
let ``Gets imperatives - multiple options``() =
    "orat"
    |> getArticle
    |> VerbArticleWithImperative
    |> getImperatives
    |> seqEquals ["oř"; "orej"]

[<Fact>]
let ``Gets participle from the second table``() = 
    "uvidět"
    |> getArticle
    |> VerbArticleWithParticiple
    |> getParticiples
    |> seqEquals ["uviděl"]

[<Fact>]
let ``Gets participle from the third table``() = 
    "myslet"
    |> getArticle
    |> VerbArticleWithParticiple
    |> getParticiples
    |> seqEquals ["myslel"]

[<Fact>]
let ``Gets all conjugations``() =
    let conjugation = 
        "myslet" 
        |> getArticle 
        |> VerbArticleWithConjugation 
        |> getConjugation
    
    conjugation.FirstSingular |> seqEquals ["myslím"]
    conjugation.SecondSingular |> seqEquals ["myslíš"]
    conjugation.ThirdSingular |> seqEquals ["myslí"]

    conjugation.FirstPlural |> seqEquals ["myslíme"]
    conjugation.SecondPlural |> seqEquals ["myslíte"]
    conjugation.ThirdPlural |> seqEquals [ "myslí"; "myslejí"]

[<Theory>]
[<InlineData "spát">]
[<InlineData "milovat">]
let ``Detects valid word for conjugations`` word =
    word
    |> getArticle
    |> parseVerbConjugation
    |> Option.isSome
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "nový">]
let ``Detects invalid word for conjugations`` word =
    word
    |> getArticle
    |> parseVerbConjugation
    |> Option.isSome
    |> Assert.False

[<Theory>]
[<InlineData "spát">]
[<InlineData "milovat">]
let ``Detects valid word for imperatives`` word =
    word
    |> getArticle
    |> parseVerbImperative
    |> Option.isSome
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "nový">]
let ``Detects invalid word for imperatives`` word =
    word
    |> getArticle
    |> parseVerbImperative
    |> Option.isSome
    |> Assert.False

[<Theory>]
[<InlineData "spát">]
[<InlineData "milovat">]
let ``Detects valid word for participles`` word =
    word
    |> getArticle
    |> parseVerbParticiple
    |> Option.isSome
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "nový">]
let ``Detects invalid word for participles`` word =
    word
    |> getArticle
    |> parseVerbParticiple
    |> Option.isSome
    |> Assert.False
