module Common.GrammarCategories.Nouns

type Declinability =
    | Declinable
    | Indeclinable

type Gender =
    | MasculineAnimate
    | MasculineInanimate
    | Feminine
    | Neuter

type Declension = {
    SingularNominative: seq<string>
    SingularGenitive: seq<string>
    SingularDative: seq<string>
    SingularAccusative: seq<string>
    SingularVocative: seq<string>
    SingularLocative: seq<string>
    SingularInstrumental: seq<string>
    PluralNominative: seq<string>
    PluralGenitive: seq<string>
    PluralDative: seq<string>
    PluralAccusative: seq<string>
    PluralVocative: seq<string>
    PluralLocative: seq<string>
    PluralInstrumental: seq<string>
}

type MasculineAnimateDeclensionPattern = 
    | Pán
    | Muž
    | Předseda
    | Soudce
    | Dinosaurus

type MasculineInanimateDeclensionPattern = 
    | Hrad
    | Stroj
    | Rytmus

type FeminineDeclensionPattern = 
    | Žena
    | Růže
    | Píseň
    | Kost

type NeuterDeclensionPattern = 
    | Město
    | Moře
    | Stavení
    | Kuře
    | Drama
    | Muzeum

type DeclensionPattern = 
    | MasculineAnimate of MasculineAnimateDeclensionPattern
    | MasculineInanimate of MasculineInanimateDeclensionPattern
    | Feminine of FeminineDeclensionPattern
    | Neuter of NeuterDeclensionPattern
