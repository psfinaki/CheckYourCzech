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

type VerbClass = E | NE | JE | Í | Á

type ConjugationPatternClassE =
   | Nést
   | Číst
   | Péct
   | Třít
   | Brát
   | Mazat

type ConjugationPatternClassNE =
    | Tisknout
    | Minout
    | Začít

type ConjugationPatternClassJE =
    | Kupovat
    | Krýt

type ConjugationPatternClassÍ =
    | Prosit
    | Čistit
    | Trpět
    | Sázet

type ConjugationPatternClassÁ =
    | Dělat

type ConjugationPattern =
    | ClassE of ConjugationPatternClassE
    | ClassNE of ConjugationPatternClassNE
    | ClassJE of ConjugationPatternClassJE
    | ClassÍ of ConjugationPatternClassÍ
    | ClassÁ of ConjugationPatternClassÁ
