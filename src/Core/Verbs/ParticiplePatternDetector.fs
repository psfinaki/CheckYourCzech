module ParticiplePatternDetector

open VerbPatternDetector

type Pattern = 
    | Minout
    | Tisknout
    | Common

let getPattern = function
    | verb when verb |> isPatternMinout -> Minout
    | verb when verb |> isPatternTisknout -> Tisknout
    | _ -> Common
