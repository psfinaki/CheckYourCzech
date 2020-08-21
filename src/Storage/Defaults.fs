module Storage.Defaults

open Common
open Common.Exercises

type Noun with 
    static member Default = {
        CanonicalForm = null
        Gender = None
        Patterns = Seq.empty
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

type AdjectiveDeclension with
    static member Default = {
        CanonicalForm = null
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
        Singular = null
        Plural = null
    }

type AdjectiveComparative with 
    static member Default = {
        Positive = null
        Comparatives = null
        IsRegular = false
    }

type VerbImperative with 
    static member Default = {
        Indicative = null
        Imperatives = null
        Class = None
        Pattern = None
    }

type VerbParticiple with 
    static member Default = {
        Infinitive = null
        Participles = null
        Pattern = Verbs.ParticiplePattern.Common
        IsRegular = false
    }

type VerbConjugation with 
    static member Default = {
        Infinitive = null
        Class = None
        Pattern = None
        Conjugation = {
            FirstSingular = null
            SecondSingular = null
            ThirdSingular = null
            FirstPlural = null
            SecondPlural = null
            ThirdPlural = null
        }
    }
