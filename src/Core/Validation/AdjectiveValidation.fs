module AdjectiveValidation

open Article
open AdjectiveArticle
open Comparison
open ComparativeBuilder

let hasMorphologicalComparatives = 
    getComparatives 
    >> Array.filter isMorphologicalComparison
    >> (not << Seq.isEmpty)

let tryGetAdjective =
    tryGetContent
    >> Option.filter (hasChildPart "čeština")
    >> Option.map (getChildPart "čeština")
    >> Option.filter (hasChildPart "přídavné jméno")
    >> Option.map (getChildPart "přídavné jméno")

let hasRequiredInfoComparative =
    tryGetAdjective
    >> Option.exists (hasChildPart "stupňování")

let hasRequiredInfoPlural = 
    tryGetAdjective 
    >> Option.exists (hasChildPart "skloňování")

let isValidAdjective = isPositive

let isComparativeValid word = 
    word |> isValidAdjective &&
    word |> hasRequiredInfoComparative &&
    word |> hasMorphologicalComparatives &&
    word |> canBuildComparative

let isPluralValid word = 
    word |> isValidAdjective &&
    word |> hasRequiredInfoPlural
