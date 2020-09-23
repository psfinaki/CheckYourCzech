module Common.Exercises

open Common.GrammarCategories

type Noun = {
    CanonicalForm: string
    Gender: Nouns.Gender option
    Patterns: seq<Nouns.DeclensionPattern>
    Declension: Nouns.Declension
}

type AdjectiveDeclension = {
    CanonicalForm: string
    Declension: Adjectives.Declension
}

type AdjectiveComparative = {
    Positive: string
    Comparatives: seq<string>
    IsRegular: bool
}

type VerbImperative = {
    Indicative: string
    Imperatives: seq<string>
    Class: Verbs.VerbClass option
    Pattern: Verbs.ConjugationPattern option
}

type VerbParticiple = {
    Infinitive: string
    Participles: seq<string>
    Pattern: Verbs.ParticiplePattern
    IsRegular: bool
}

type VerbConjugation = {
    Infinitive: string
    Class: Verbs.VerbClass option
    Pattern: Verbs.ConjugationPattern option
    Conjugation: Verbs.Conjugation
}
