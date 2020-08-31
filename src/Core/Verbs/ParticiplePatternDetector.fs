module Core.Verbs.ParticiplePatternDetector

open ConjugationPatternDetector
open Common.GrammarCategories.Verbs

let getPattern = function
    | verb when verb |> isPatternMinout -> ParticiplePattern.Minout
    | verb when verb |> isPatternTisknout -> ParticiplePattern.Tisknout
    | _ -> ParticiplePattern.Common
