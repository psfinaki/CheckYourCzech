module Common.Translations

open Common.GrammarCategories

let genderFromString = function
    | "rod mužský životný" -> Nouns.Gender.MasculineAnimate
    | "rod mužský neživotný" -> Nouns.Gender.MasculineInanimate
    | "rod ženský" -> Nouns.Gender.Feminine
    | "rod střední" -> Nouns.Gender.Neuter
    | _ -> failwith "Unknown gender"

let genderToString = function
    | Nouns.Gender.MasculineAnimate -> "rod mužský životný"
    | Nouns.Gender.MasculineInanimate -> "rod mužský neživotný"
    | Nouns.Gender.Feminine -> "rod ženský"
    | Nouns.Gender.Neuter -> "rod střední"

let pronounToString = function
    | Verbs.Pronoun.FirstSingular   -> "já"
    | Verbs.Pronoun.SecondSingular  -> "ty"
    | Verbs.Pronoun.ThirdSingular   -> "ono"
    | Verbs.Pronoun.FirstPlural     -> "my"
    | Verbs.Pronoun.SecondPlural    -> "vy"
    | Verbs.Pronoun.ThirdPlural     -> "oni"
