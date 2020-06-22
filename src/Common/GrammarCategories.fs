module Common.GrammarCategories

type Case = 
    | Nominative
    | Genitive
    | Dative
    | Accusative
    | Vocative
    | Locative
    | Instrumental

type Number =
    | Singular
    | Plural

type Gender =
    | MasculineAnimate
    | MasculineInanimate
    | Feminine
    | Neuter

type Declinability =
    | Declinable
    | Indeclinable

type Person = 
    | First | Second | Third
