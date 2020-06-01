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
    let verb = "myslet"
    let article = verb |> getArticle |> VerbArticleWithConjugation
    
    getConjugation FirstSingular article  |> seqEquals ["myslím"]
    getConjugation SecondSingular article |> seqEquals ["myslíš"]
    getConjugation ThirdSingular article  |> seqEquals ["myslí"]

    getConjugation FirstPlural article    |> seqEquals ["myslíme"]
    getConjugation SecondPlural article   |> seqEquals ["myslíte"]
    getConjugation ThirdPlural article    |> seqEquals [ "myslí"; "myslejí"]
