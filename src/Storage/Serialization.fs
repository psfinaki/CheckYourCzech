module Storage.Serialization

open Common.Declension
open Common.StringHelper

type DeclensionPattern with
    static member toString = function
        | MasculineAnimate pattern -> pattern |> string |> toLower
        | MasculineInanimate pattern -> pattern |> string |> toLower
        | Feminine pattern -> pattern |> string |> toLower
        | Neuter pattern -> pattern |> string |> toLower
