module Adjective

open WikiArticles

let getPlural =
    AdjectiveArticle.getPlural

let getComparatives = 
    AdjectiveArticle.getComparatives

let hasRegularComparative article =
    let (AdjectiveArticleWithComparative (AdjectiveArticle { Title = word })) = article

    let theoretical = ComparativeBuilder.buildComparative word
    let practical = getComparatives article
    practical |> Array.contains theoretical
