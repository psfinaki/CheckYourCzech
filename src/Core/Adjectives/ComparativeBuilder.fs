module Core.Adjectives.ComparativeBuilder

open Core.Stem
open Common.StringHelper

let getStem (adjective: string) = adjective.TrimEnd('í', 'ý')

let canBuildComparative = 
    getStem
    >> (not << ends "c")

let addComparativeSuffix = function
    | stem when stem |> endsHard -> $"{stem}ější"
    | stem when stem |> endsSoft -> $"{stem}ejší"
    | stem -> invalidArg stem "odd stem"

let buildComparative = 
    getStem
    >> alternate
    >> addComparativeSuffix
