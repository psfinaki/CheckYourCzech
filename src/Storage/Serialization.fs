module Storage.Serialization

open Common.Conjugation
open Common.Declension
open Common.StringHelper

type DeclensionPattern with
    static member toString = function
        | MasculineAnimate pattern -> pattern |> string |> toLower
        | MasculineInanimate pattern -> pattern |> string |> toLower
        | Feminine pattern -> pattern |> string |> toLower
        | Neuter pattern -> pattern |> string |> toLower

type ConjugationPattern with
    static member toString = function
        | ClassE pattern -> pattern |> string |> toLower
        | ClassNE pattern -> pattern |> string |> toLower
        | ClassJE pattern -> pattern |> string |> toLower
        | ClassÍ  pattern -> pattern |> string |> toLower
        | ClassÁ pattern -> pattern |> string |> toLower
