module GrammarCategories

type Case = 
    | Nominative = 0
    | Genitive = 1
    | Dative = 2
    | Accusative = 3
    | Vocative = 4
    | Locative = 5
    | Instrumental = 6

type Number =
    | Singular
    | Plural

type Declinability =
    | Declinable
    | Indeclinable
