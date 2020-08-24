module Storage.Serialization

open Common.GrammarCategories
open Common.StringHelper

type Nouns.DeclensionPattern with
    static member toString = function
        | Nouns.DeclensionPattern.MasculineAnimate pattern -> pattern |> string |> toLower
        | Nouns.DeclensionPattern.MasculineInanimate pattern -> pattern |> string |> toLower
        | Nouns.DeclensionPattern.Feminine pattern -> pattern |> string |> toLower
        | Nouns.DeclensionPattern.Neuter pattern -> pattern |> string |> toLower

type Verbs.ConjugationPattern with
    static member toString = function
        | Verbs.ConjugationPattern.ClassE pattern -> pattern |> string |> toLower
        | Verbs.ConjugationPattern.ClassNE pattern -> pattern |> string |> toLower
        | Verbs.ConjugationPattern.ClassJE pattern -> pattern |> string |> toLower
        | Verbs.ConjugationPattern.ClassÍ  pattern -> pattern |> string |> toLower
        | Verbs.ConjugationPattern.ClassÁ pattern -> pattern |> string |> toLower
