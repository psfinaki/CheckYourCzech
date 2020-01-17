module GenderTranslations

open GrammarCategories

let fromString = function
    | "rod mužský životný" -> MasculineAnimate
    | "rod mužský neživotný" -> MasculineInanimate
    | "rod ženský" -> Feminine
    | "rod střední" -> Neuter
    | _ -> failwith "Unknown gender"

let toString = function
    | MasculineAnimate -> "rod mužský životný"
    | MasculineInanimate -> "rod mužský neživotný"
    | Feminine -> "rod ženský"
    | Neuter -> "rod střední"

