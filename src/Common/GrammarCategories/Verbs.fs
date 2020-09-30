module Common.GrammarCategories.Verbs

type Pronoun =
    | FirstSingular 
    | SecondSingular 
    | ThirdSingular 
    | FirstPlural 
    | SecondPlural 
    | ThirdPlural

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

type ParticiplePattern = 
    | Minout
    | Tisknout
    | Common

type Conjugation = {
    Infinitive: string
    FirstSingular: seq<string>
    SecondSingular: seq<string>
    ThirdSingular: seq<string>
    FirstPlural: seq<string>
    SecondPlural: seq<string>
    ThirdPlural: seq<string>
}

type Imperative = {
    Indicative: string
    Imperatives: seq<string>
}

type Participle = {
    Infinitive: string
    Participles: seq<string>
}
