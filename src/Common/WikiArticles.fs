module Common.WikiArticles

open Common.GrammarCategories

type Article = {
    Title: string
    Text: string 
}

type NounArticle = NounArticle of Article
type AdjectiveArticle = AdjectiveArticle of Article
type VerbArticle = VerbArticle of Article

type Noun = {
    CanonicalForm: string
    Declinability: Nouns.Declinability
    Gender: Nouns.Gender option
    Declension: Nouns.Declension option
}

type Adjective = {
    CanonicalForm: string
    Declension: Adjectives.Declension option
    Comparison: Adjectives.Comparison option
}

type VerbConjugation = {
    Infinitive: string
    Conjugation: Verbs.Conjugation
}

type Verb = {
    CanonicalForm: string
    Conjugation: VerbConjugation option
    Imperative: Verbs.Imperative option
    Participle: Verbs.Participle option
}
