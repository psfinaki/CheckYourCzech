module Common.Exercises

open Declension
open Conjugation
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
    Class: VerbClass option
    Pattern: ConjugationPattern option
}

type VerbParticiple = {
    Infinitive: string
    Participles: seq<string>
    Pattern: Verbs.ParticiplePattern
    IsRegular: bool
}

type VerbConjugation = {
    Infinitive: string
    Pattern: Verbs.ParticiplePattern
    Conjugation: Conjugation.Conjugation
}
