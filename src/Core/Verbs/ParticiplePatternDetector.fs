module Core.Verbs.ParticiplePatternDetector

open ConjugationPatternDetector
open Common.Verbs

let getPattern = function
    | verb when verb |> isPatternMinout -> Minout
    | verb when verb |> isPatternTisknout -> Tisknout
    | _ -> Common
