module Adjective

let getPlural = 
    AdjectiveArticle.getPlural

let getComparatives = 
    AdjectiveArticle.getComparatives

let hasRegularComparative word =
    let theoretical = ComparativeBuilder.buildComparative word
    let practical = getComparatives word
    practical |> Array.contains theoretical
