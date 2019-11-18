module VerbArticleTests

open Xunit
open VerbArticle
open GrammarCategories
open System.Collections.Generic

[<Fact>]
let ``Gets imperatives - single option``() =
    "milovat"
    |> getImperatives
    |> equals [|"miluj"|]

[<Fact>]
let ``Gets imperatives - multiple options``() =
    "orat"
    |> getImperatives
    |> equals [|"oř"; "orej"|]

[<Fact>]
let ``Gets participle from the second table``() = 
    "uvidět"
    |> getParticiples
    |> equals [|"uviděl"|]

[<Fact>]
let ``Gets participle from the third table``() = 
    "myslet"
    |> getParticiples
    |> equals [|"myslel"|]

[<Fact>]
let ``Gets all conjugations``() = 
    let conj = "myslet" |> getConjugations
    let getKeyValue (kv:KeyValuePair<'a, 'b>) = 
        kv.Key, kv.Value
    
    conj.Item(Singular, First) |> equals [|"myslím"|]
    conj.Item(Singular, Second) |> equals [|"myslíš"|]
    conj.Item(Singular, Third) |> equals [|"myslí"|]

    conj.Item(Plural, First) |> equals [|"myslíme"|]
    conj.Item(Plural, Second) |> equals [|"myslíte"|]
    conj.Item(Plural, Third) |> equals [| "myslí"; "myslejí"|]
