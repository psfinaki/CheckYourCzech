module Common.WikiArticles

type Article = {
    Title: string
    Text: string 
}

type NounArticle = NounArticle of Article

type AdjectiveArticle = AdjectiveArticle of Article
type AdjectiveArticleWithPlural = AdjectiveArticleWithPlural of AdjectiveArticle
type AdjectiveArticleWithComparative = AdjectiveArticleWithComparative of AdjectiveArticle

type VerbArticle = VerbArticle of Article
type VerbArticleWithImperative = VerbArticleWithImperative of VerbArticle
type VerbArticleWithParticiple = VerbArticleWithParticiple of VerbArticle
type VerbArticleWithConjugation = VerbArticleWithConjugation of VerbArticle

type AdjectiveDeclension = {
    Singular: string
    Plural: string
}

type AdjectiveComparison = {
    Positive: string
    Comparatives: seq<string>
}

type Adjective = {
    CanonicalForm: string
    Declension: AdjectiveDeclension option
    Comparison: AdjectiveComparison option
}
