module NounArticleTests

open Xunit
open NounArticle
open GrammarCategories
open WikiArticles

let getArticle =
    Article.getArticle
    >> Option.get
    >> NounArticle

[<Fact>]
let ``Gets number of declensions``() =
    "starost"
    |> getArticle
    |> getNumberOfDeclensions
    |> equals 1

[<Fact>]
let ``Gets declension wiki - indeclinable``() = 
    "dada"
    |> getArticle
    |> getDeclensionWiki Case.Nominative Number.Plural
    |> equals ["dada"]

[<Fact>]
let ``Gets declension wiki - editable article``() = 
    "panda"
    |> getArticle
    |> getDeclensionWiki Case.Nominative Number.Plural
    |> equals ["pandy"]

[<Fact>]
let ``Gets wiki plural wiki - locked article``() = 
    "debil"
    |> getArticle
    |> getDeclensionWiki Case.Nominative Number.Plural
    |> equals ["debilové"]

[<Fact>]
let ``Gets declension - indeclinable``() =
    "dada"
    |> getArticle
    |> getDeclension Case.Nominative Number.Singular
    |> seqEquals ["dada"]

[<Fact>]
let ``Gets declension - single option``() =
    "hrad"
    |> getArticle
    |> getDeclension Case.Nominative Number.Singular
    |> seqEquals ["hrad"]

[<Fact>]
let ``Gets declension - no options``() =
    "záda"
    |> getArticle
    |> getDeclension Case.Nominative Number.Singular
    |> seqEquals []

[<Fact>]
let ``Gets declension - multiple declensions``() =
    "čtvrt"
    |> getArticle
    |> getDeclension Case.Nominative Number.Plural
    |> seqEquals ["čtvrtě"; "čtvrti"]

[<Fact>]
let ``Gets singulars - multiple options``() =
    "temeno"
    |> getArticle
    |> getDeclension Case.Nominative Number.Singular
    |> seqEquals ["temeno"; "témě"]
    
[<Theory>]
[<InlineData "dada">]
[<InlineData "karé">]
let ``Detects indeclinable`` word =
    word
    |> getArticle
    |> getDeclinability
    |> equals Indeclinable
    
[<Fact>]
let ``Detects declinable``() =
    "panda"
    |> getArticle
    |> getDeclinability
    |> equals Declinable

[<Theory>]
[<InlineData "panda">]
[<InlineData "lipnice">]
[<InlineData "krajta">]
let ``Gets gender`` word =
    word
    |> getArticle
    |> getGender
    |> equals (Some "rod ženský")
