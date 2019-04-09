module BoolExtensioins

open System

type Boolean with
    
    // Fable cannot compile bool.Parse
    static member FromString = function
        | "True" -> true
        | "False" -> false
        | _ -> invalidOp "Value cannot be converted to boolean."
        
    // ToString() for bool does not work in Fable
    static member AsString = function
        | true -> "True"
        | false -> "False"
