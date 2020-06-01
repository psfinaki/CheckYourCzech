module Storage.Defaults

open Common
open Common.Exercises

type Noun with 
    static member Default = {
        Id = null
        Gender = None
        Patterns = null
        Declension = {
            SingularNominative = null
            SingularGenitive = null
            SingularDative = null
            SingularAccusative = null
            SingularVocative = null
            SingularLocative = null
            SingularInstrumental = null
            PluralNominative = null
            PluralGenitive = null
            PluralDative = null
            PluralAccusative = null
            PluralVocative = null
            PluralLocative = null
            PluralInstrumental = null
        }
    }

type AdjectivePlural with 
    static member Default = {
        Id = null
        Singular = null
        Plural = null
    }

type AdjectiveComparative with 
    static member Default = {
        Id = null
        Positive = null
        Comparatives = null
        IsRegular = false
    }

type VerbImperative with 
    static member Default = {
        Id = null
        Indicative = null
        Imperatives = null
        Class = None
        Pattern = None
    }

type VerbParticiple with 
    static member Default = {
        Id = null
        Infinitive = null
        Participles = null
        Pattern = Verbs.Pattern.Common
        IsRegular = false
    }

type VerbConjugation with 
    static member Default = {
        Id = null
        Infinitive = null
        Pattern = Verbs.Pattern.Common
        Conjugation = {
            FirstSingular = null
            SecondSingular = null
            ThirdSingular = null
            FirstPlural = null
            SecondPlural = null
            ThirdPlural = null
        }
    }
