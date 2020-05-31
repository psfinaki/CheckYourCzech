module Common.Exercises

open GrammarCategories

type Noun = {
    Id: string
    Gender: Gender option
    Patterns: seq<string>
    Declension: Declension
}

type AdjectivePlural = {
    Id: string
    Singular: string
    Plural: string
}

type AdjectiveComparative = {
    Id: string
    Positive: string
    Comparatives: seq<string>
    IsRegular: bool
}

type VerbImperative = {
    Id: string
    Indicative: string
    Imperatives: seq<string>
    Class: Verbs.VerbClass option
    Pattern: string option
}

type VerbParticiple = {
    Id: string
    Infinitive: string
    Participles: seq<string>
    Pattern: Verbs.Pattern
    IsRegular: bool
}

type VerbConjugation = {
    Id: string
    Infinitive: string
    Pattern: Verbs.Pattern
    Conjugation: Conjugation.Conjugation
}
