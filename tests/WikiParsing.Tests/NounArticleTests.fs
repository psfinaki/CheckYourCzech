module WikiParsing.Tests.NounArticleTests

open Xunit

open WikiParsing.Articles.NounArticle
open Common.GrammarCategories.Nouns
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
let ``Gets declension - editable article``() = 
    "panda"
    |> getDeclension
    |> fun declension -> declension.PluralNominative
    |> seqEquals ["pandy"]

[<Fact>]
let ``Gets declension - locked article``() = 
    "debil"
    |> getDeclension
    |> fun declnesion -> declnesion.PluralNominative
    |> seqEquals ["debilové"]

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

[<Theory>]
[<InlineData "láska">]
[<InlineData "kachna">]
[<InlineData "nůžky">]
let ``Detects valid word for declension`` word =
    word
    |> getArticle
    |> hasRequiredInfoDeclension
    |> Assert.True

[<Theory>]
[<InlineData "hello">]
[<InlineData "nový">]
[<InlineData "vrchní">]
let ``Detects invalid word for declension`` word =
    word
    |> getArticle
    |> hasRequiredInfoDeclension
    |> Assert.False
