module Word

open Article

let recordCzechPartOfSpeech word = function
    | "podstatné jméno" -> Noun.record word
    | "přídavné jméno"  -> Adjective.record word
    | "sloveso"         -> Verb.record word
    | _ -> ()
    
let recordCzechPart word =
    getPartNames
    >> Seq.iter (recordCzechPartOfSpeech word)

let recordLanguagePart word = function
    | ("čeština", part) -> recordCzechPart word part
    | _ -> ()

let record word =
    word
    |> getContent 
    |> getPartsWithNames
    |> Seq.iter (recordLanguagePart word)
