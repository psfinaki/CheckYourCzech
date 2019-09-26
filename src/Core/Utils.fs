module Utils

let boolOptionToBool opt = 
    match opt with 
    | Some x -> x
    | None   -> false