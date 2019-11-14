module NounArticleTests

open Xunit
open NounArticle
open GrammarCategories

[<Fact>]
let ``Gets number of declensions``() =
    "starost"
    |> getNumberOfDeclensions
    |> equals 1

[<Fact>]
let ``Gets declension wiki - indeclinable``() = 
    "dada"
    |> getDeclensionWiki Case.Nominative Number.Plural
    |> equals ["dada"]

[<Fact>]
let ``Gets declension wiki - editable article``() = 
    "panda"
    |> getDeclensionWiki Case.Nominative Number.Plural
    |> equals ["pandy"]

[<Fact>]
let ``Gets wiki plural wiki - locked article``() = 
    "debil"
    |> getDeclensionWiki Case.Nominative Number.Plural
    |> equals ["debilové"]

[<Fact>]
let ``Gets declension - indeclinable``() =
    "dada"
    |> getDeclension Case.Nominative Number.Singular
    |> seqEquals ["dada"]

[<Fact>]
let ``Gets declension - single option``() =
    "hrad"
    |> getDeclension Case.Nominative Number.Singular
    |> seqEquals ["hrad"]

[<Fact>]
let ``Gets declension - no options``() =
    "záda"
    |> getDeclension Case.Nominative Number.Singular
    |> seqEquals []

[<Fact>]
let ``Gets declension - multiple declensions``() =
    "čtvrt"
    |> getDeclension Case.Nominative Number.Plural
    |> seqEquals ["čtvrtě"; "čtvrti"]

[<Fact>]
let ``Gets singulars - multiple options``() =
    "temeno"
    |> getDeclension Case.Nominative Number.Singular
    |> seqEquals ["temeno"; "témě"]
    
[<Theory>]
[<InlineData "dada">]
[<InlineData "karé">]
let ``Detects indeclinable`` word =
    word
    |> getDeclinability
    |> equals Indeclinable
    
[<Fact>]
let ``Detects declinable``() =
    "panda"
    |> getDeclinability
    |> equals Declinable

[<Fact>]
let ``Gets gender``() =
    "panda"
    |> getGender
    |> equals "rod ženský"
