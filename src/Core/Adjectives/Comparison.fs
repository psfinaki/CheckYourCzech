module Comparison

open StringHelper

let isHardPositive = endsOneOf ["ý"; "á"; "é"]

let isSoftPositive adjective = 
    adjective |> ends "í" && 
    adjective |> (not << ends "ší")

let isPositive adjective = 
    adjective |> isHardPositive || 
    adjective |> isSoftPositive

let isSyntacticComparison = starts "více "

let isMorphologicalComparison = not << isSyntacticComparison

