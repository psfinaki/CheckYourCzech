module Common.Exercises

open Declension
open GrammarCategories

type Noun = {
    Gender: Gender option
    Patterns: seq<DeclensionPattern>
    Declension: Declension
}

type AdjectivePlural = {
    Singular: string
    Plural: string
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
    Pattern: string option
}

type VerbParticiple = {
    Infinitive: string
    Participles: seq<string>
    Pattern: Verbs.Pattern
    IsRegular: bool
}

type VerbConjugation = {
    Infinitive: string
    Pattern: Verbs.Pattern
    Conjugation: Conjugation.Conjugation
}
