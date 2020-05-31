module Core.Verbs.ParticiplePatternDetector

open VerbPatternDetector
open Common.Verbs

let getPattern = function
    | verb when verb |> isPatternMinout -> Minout
    | verb when verb |> isPatternTisknout -> Tisknout
    | _ -> Common
