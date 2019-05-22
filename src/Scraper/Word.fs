module Word

open Article

let recordCzechPartOfSpeech word = function
    | "podstatné jméno" ->
        NounPlural.record word
        NounAccusative.record word
    | "přídavné jméno"  ->
        AdjectivePlural.record word
        AdjectiveComparative.record word
    | "sloveso"         -> 
        VerbImperative.record word
        VerbParticiple.record word
    | _ -> ()
    
let recordCzechPart word = 
    getChildrenParts 
    >> Seq.map fst
    >> Seq.iter (recordCzechPartOfSpeech word)

let recordLanguagePart word = function
    | ("čeština", part) -> recordCzechPart word part
    | _ -> ()

let record word =
    word
    |> getContent 
    |> getChildrenParts
    |> Seq.iter (recordLanguagePart word)
