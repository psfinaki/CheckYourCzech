module Core.Adjectives.Adjective

open Common.WikiArticles
open WikiParsing.Articles
open ComparativeBuilder

let getPlural =
    AdjectiveArticle.getPlural

let getComparatives = 
    AdjectiveArticle.getComparatives

let hasRegularComparative article =
    let (AdjectiveArticleWithComparative (AdjectiveArticle { Title = word })) = article

    let theoretical = buildComparative word
    let practical = getComparatives article
    practical |> Seq.contains theoretical
