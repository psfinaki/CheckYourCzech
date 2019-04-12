module ParticiplePatternDetector

open VerbPatternDetector

type Pattern = 
    | Minout
    | Tisknout
    | Other

let getPatternNonReflexive = function
    | verb when verb |> isPatternMinout -> Minout
    | verb when verb |> isPatternTisknout -> Tisknout
    | _ -> Other

let getPattern = Verb.removeReflexive >> getPatternNonReflexive
