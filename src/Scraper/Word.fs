module Word

open Article

let recordCzechPartOfSpeech word = function
    | "podstatné jméno" -> [
        if word |> NounValidation.isPluralValid
        then word |> NounRegistration.registerNounPlural

        if word |> NounValidation.isAccusativeValid
        then word |> NounRegistration.registerNounAccusative
      ]

    | "přídavné jméno" -> [
        if word |> AdjectiveValidation.isPluralValid
        then word |> AdjectiveRegistration.registerAdjectivePlural

        if word |> AdjectiveValidation.isComparativeValid
        then word |> AdjectiveRegistration.registerAdjectiveComparative
      ]
            
    | "sloveso" -> [
        if word |> VerbValidation.isImperativeValid
        then word |> VerbRegistration.registerVerbImperative

        if word |> VerbValidation.isParticipleValid
        then word |> VerbRegistration.registerVerbParticiple

        if word |> VerbValidation.isConjugationValid
        then word |> VerbRegistration.registerVerbConjugation
      ]

    | _ -> []
    
let record word =
    word
    |> getPartsOfSpeech
    |> Seq.collect (recordCzechPartOfSpeech word)
    |> Async.Parallel
    |> Async.Ignore
    |> Async.RunSynchronously
