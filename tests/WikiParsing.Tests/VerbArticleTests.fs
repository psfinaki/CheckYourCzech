module VerbArticleTests

open Xunit
open VerbArticle
open Conjugation
open WikiArticles

let getArticle =
    Article.getArticle
    >> Option.get
    >> VerbArticle

[<Fact>]
let ``Gets imperatives - single option``() =
    "milovat"
    |> getArticle 
    |> VerbArticleWithImperative
    |> getImperatives
    |> equals [|"miluj"|]

[<Fact>]
let ``Gets imperatives - multiple options``() =
    "orat"
    |> getArticle
    |> VerbArticleWithImperative
    |> getImperatives
    |> equals [|"oř"; "orej"|]

[<Fact>]
let ``Gets participle from the second table``() = 
    "uvidět"
    |> getArticle
    |> VerbArticleWithParticiple
    |> getParticiples
    |> equals [|"uviděl"|]

[<Fact>]
let ``Gets participle from the third table``() = 
    "myslet"
    |> getArticle
    |> VerbArticleWithParticiple
    |> getParticiples
    |> equals [|"myslel"|]

[<Fact>]
let ``Gets all conjugations``() =
    let verb = "myslet"
    let article = verb |> getArticle |> VerbArticleWithConjugation
    
    getConjugation FirstSingular article  |> equals [|"myslím"|]
    getConjugation SecondSingular article |> equals [|"myslíš"|]
    getConjugation ThirdSingular article  |> equals [|"myslí"|]

    getConjugation FirstPlural article    |> equals [|"myslíme"|]
    getConjugation SecondPlural article   |> equals [|"myslíte"|]
    getConjugation ThirdPlural article    |> equals [| "myslí"; "myslejí"|]
