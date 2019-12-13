module Conjugation

type Pronoun =
    | FirstSingular 
    | SecondSingular 
    | ThirdSingular 
    | FirstPlural 
    | SecondPlural 
    | ThirdPlural

let pronounToString pronoun =
    match pronoun with
    | FirstSingular   -> "já"
    | SecondSingular  -> "ty"
    | ThirdSingular   -> "ono"
    | FirstPlural     -> "my"
    | SecondPlural    -> "vy"
    | ThirdPlural     -> "oni"