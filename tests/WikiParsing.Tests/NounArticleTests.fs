module WikiParsing.Tests.NounArticleTests

open Xunit

open WikiParsing.Articles.NounArticle
open Common.GrammarCategories
open Common.WikiArticles

let getArticle =
    getArticle
    >> NounArticle

let getDeclension = getArticle >> getDeclension

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
    |> getDeclension
    |> fun declension -> declension.SingularNominative
    |> seqEquals ["dada"]

[<Fact>]
let ``Gets declension - single option``() =
    "hrad"
    |> getDeclension
    |> fun declension -> declension.SingularNominative
    |> seqEquals ["hrad"]

[<Fact>]
let ``Gets declension - no options``() =
    "záda"
    |> getDeclension
    |> fun declension -> declension.SingularNominative
    |> seqEquals []

[<Fact>]
let ``Gets declension - multiple declensions``() =
    "čtvrt"
    |> getDeclension
    |> fun declension -> declension.PluralNominative
    |> seqEquals ["čtvrtě"; "čtvrti"]

[<Fact>]
let ``Gets singulars - multiple options``() =
    "temeno"
    |> getDeclension
    |> fun declension -> declension.SingularNominative
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
