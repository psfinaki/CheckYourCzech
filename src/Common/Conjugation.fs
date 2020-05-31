module Common.Conjugation

type Pronoun =
    | FirstSingular 
    | SecondSingular 
    | ThirdSingular 
    | FirstPlural 
    | SecondPlural 
    | ThirdPlural

let pronounToString = function
    | FirstSingular   -> "já"
    | SecondSingular  -> "ty"
    | ThirdSingular   -> "ono"
    | FirstPlural     -> "my"
    | SecondPlural    -> "vy"
    | ThirdPlural     -> "oni"

type Conjugation = {
    FirstSingular: seq<string>
    SecondSingular: seq<string>
    ThirdSingular: seq<string>
    FirstPlural: seq<string>
    SecondPlural: seq<string>
    ThirdPlural: seq<string>
}
