module Common.Verbs

type VerbClass = E | NE | JE | Í | Á with 
    static member parse = function
        | "E" -> E
        | "NE" -> NE
        | "JE" -> JE
        | "Í" -> Í
        | "Á" -> Á
        | s -> invalidOp ("odd class: " + s)

type Pattern = 
    | Minout
    | Tisknout
    | Common
