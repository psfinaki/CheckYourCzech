module Common.WikiArticles

open Conjugation
open GrammarCategories

type Article = {
    Title: string
    Text: string 
}

type NounArticle = NounArticle of Article
type AdjectiveArticle = AdjectiveArticle of Article
type VerbArticle = VerbArticle of Article

type NounDeclension = {
    CanonicalForm: string
    Declinability: Declinability
    Gender: Gender option
    Declension: Declension option
}

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

type VerbConjugation = {
    Infinitive: string
    Conjugation: Conjugation
}

type VerbImperative = {
    Indicative: string
    Imperatives: seq<string>
}

type VerbParticiple = {
    Infinitive: string
    Participles: seq<string>
}

type Verb = {
    CanonicalForm: string
    Conjugation: VerbConjugation option
    Imperative: VerbImperative option
    Participle: VerbParticiple option
}
