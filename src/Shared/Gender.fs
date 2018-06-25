module Gender

type Gender =
    | MasculineAnimate
    | MasculineInanimate
    | Feminine
    | Neuter

let translateFrom gender =
    match gender with
    | MasculineAnimate -> 
        "rod mužský životný"
    | MasculineInanimate ->
        "rod mužský neživotný"
    | Feminine ->
        "rod ženský"
    | Neuter -> 
        "rod střední"

let translateTo gender = 
    match gender with
    | "rod mužský životný" -> 
        MasculineAnimate
    | "rod mužský neživotný" -> 
        MasculineInanimate
    | "rod ženský" -> 
        Feminine
    | "rod střední" -> 
        Neuter
    | _ ->
        failwith "unknown gender"

