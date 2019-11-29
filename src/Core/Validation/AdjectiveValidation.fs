module AdjectiveValidation

open Article
open AdjectiveArticle
open Comparison
open ComparativeBuilder

let hasMorphologicalComparatives = 
    getComparatives 
    >> Array.filter isMorphologicalComparison
    >> (not << Seq.isEmpty)

let hasRequiredInfoComparative =
    isMatch [
        Is "přídavné jméno"
        Is "stupňování"
    ]

let hasRequiredInfoPlural = 
    isMatch [
        Is "přídavné jméno"
        Is "skloňování"
    ]

let isValidAdjective = isPositive

let isComparativeValid word = 
    word |> isValidAdjective &&
    word |> hasRequiredInfoComparative &&
    word |> hasMorphologicalComparatives &&
    word |> canBuildComparative

let isPluralValid word = 
    word |> isValidAdjective &&
    word |> hasRequiredInfoPlural
