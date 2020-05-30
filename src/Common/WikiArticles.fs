module WikiArticles

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
