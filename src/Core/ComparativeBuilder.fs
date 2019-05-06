module ComparativeBuilder

open Stem

let getStem (adjective: string) = adjective.TrimEnd('í', 'ý')

let canBuildComparative = 
    getStem
    >> fun stem -> stem.EndsWith "c"
    >> not

let addComparativeSuffix = function
    | stem when stem |> endsHard -> stem + "ější"
    | stem when stem |> endsSoft -> stem + "ejší"
    | stem -> invalidArg stem "odd stem"

let buildComparative = 
    getStem
    >> alternate
    >> addComparativeSuffix
