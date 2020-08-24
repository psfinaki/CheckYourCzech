module Common.GrammarCategories.Adjectives

type Declension = {
    SingularNominative: string
    SingularGenitive: string
    SingularDative: string
    SingularAccusative: string
    SingularVocative: string
    SingularLocative: string
    SingularInstrumental: string
    PluralNominative: string
    PluralGenitive: string
    PluralDative: string
    PluralAccusative: string
    PluralVocative: string
    PluralLocative: string
    PluralInstrumental: string
}

type Comparison = {
    Positive: string
    Comparatives: seq<string>
}
