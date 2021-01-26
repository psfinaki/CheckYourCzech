module Storage.Serialization

open Common.GrammarCategories
open Common.StringHelper

type Nouns.DeclensionPattern with
    static member toString = function
        | Nouns.DeclensionPattern.MasculineAnimate pattern -> $"{pattern}" |> toLower
        | Nouns.DeclensionPattern.MasculineInanimate pattern -> $"{pattern}" |> toLower
        | Nouns.DeclensionPattern.Feminine pattern -> $"{pattern}" |> toLower
        | Nouns.DeclensionPattern.Neuter pattern -> $"{pattern}" |> toLower

type Verbs.ConjugationPattern with
    static member toString = function
        | Verbs.ConjugationPattern.ClassE pattern -> $"{pattern}" |> toLower
        | Verbs.ConjugationPattern.ClassNE pattern -> $"{pattern}" |> toLower
        | Verbs.ConjugationPattern.ClassJE pattern -> $"{pattern}" |> toLower
        | Verbs.ConjugationPattern.ClassÍ  pattern -> $"{pattern}" |> toLower
        | Verbs.ConjugationPattern.ClassÁ pattern -> $"{pattern}" |> toLower
