module Common.Declension

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
