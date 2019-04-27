module Word

open Article

let recordCzechPartOfSpeech word = function
    | "podstatné jméno" ->
        Plural.record word
        Accusative.record word
    | "přídavné jméno"  -> Comparative.record word
    | "sloveso"         -> 
        Imperative.record word
        Participle.record word
    | _ -> ()
    
let recordCzechPart word = 
    getParts 
    >> Seq.map fst
    >> Seq.iter (recordCzechPartOfSpeech word)

let recordLanguagePart word = function
    | ("čeština", part) -> recordCzechPart word part
    | _ -> ()

let record word =
    word
    |> getContent 
    |> getParts
    |> Seq.iter (recordLanguagePart word)
