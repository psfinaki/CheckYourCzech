module ParticiplePatternDetector

open VerbPatternDetector
open Verbs

let getPattern = function
    | verb when verb |> isPatternMinout -> Minout
    | verb when verb |> isPatternTisknout -> Tisknout
    | _ -> Common
