module Core.Verbs.VerbClasses

open Common.StringHelper
open Common.GrammarCategories.Verbs

let getClassByThirdPersonSingular = function
    | form when form |> ends "á"  -> VerbClass.Á
    | form when form |> ends "í"  -> VerbClass.Í
    | form when form |> ends "je" -> VerbClass.JE
    | form when form |> ends "ne" -> VerbClass.NE
    | form when form |> ends "e"  -> VerbClass.E
    | _ -> invalidArg "verb" "Incorrect third person singular."
