module Genders

open System
open Microsoft.FSharp.Reflection

type Gender =
    | MasculineAnimate
    | MasculineInanimate
    | Feminine
    | Neuter

let czechTranslations = 
    dict [ ("rod mužský životný",   MasculineAnimate) 
           ("rod mužský neživotný", MasculineInanimate) 
           ("rod ženský",           Feminine) 
           ("rod střední",          Neuter) ]   

let duTranslations = 
    dict [ ("MasculineAnimate",   MasculineAnimate) 
           ("MasculineInanimate", MasculineInanimate) 
           ("Feminine",           Feminine) 
           ("Neuter",             Neuter) ]   

let translateGender gender = czechTranslations.[gender]

let tryTranslateGender gender = 
    try
        gender |> translateGender |> Some
    with 
        | :? InvalidOperationException -> None

let fromString gender = duTranslations.[gender]
