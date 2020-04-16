module Word

open Article

let recordCzechPartOfSpeech article = function
    | "podstatné jméno" -> [
        article
        |> NounValidation.parseNounPlural
        |> Option.map NounRegistration.registerNounPlural
        |> Option.defaultValue (async { return () })

        article
        |> NounValidation.parseNounAccusative
        |> Option.map NounRegistration.registerNounAccusative
        |> Option.defaultValue (async { return () })
      ]

    | "přídavné jméno" -> [
        article 
        |> AdjectiveValidation.parseAdjectivePlural
        |> Option.map AdjectiveRegistration.registerAdjectivePlural
        |> Option.defaultValue (async { return () })

        article 
        |> AdjectiveValidation.parseAdjectiveComparative
        |> Option.map AdjectiveRegistration.registerAdjectiveComparative
        |> Option.defaultValue (async { return () })
      ]
            
    | "sloveso" -> [
        article
        |> VerbValidation.parseVerbImperative
        |> Option.map VerbRegistration.registerVerbImperative
        |> Option.defaultValue (async { return () })

        article
        |> VerbValidation.parseVerbParticiple
        |> Option.map VerbRegistration.registerVerbParticiple
        |> Option.defaultValue (async { return () })

        article
        |> VerbValidation.parseVerbConjugation
        |> Option.map VerbRegistration.registerVerbConjugation
        |> Option.defaultValue (async { return () })
      ]

    | _ -> []
    
let getTasks article = 
    article 
    |> getPartsOfSpeech 
    |> Seq.collect (recordCzechPartOfSpeech article)

let record word =
    word
    |> getArticle
    |> Option.map getTasks
    |> Option.defaultValue Seq.empty
    |> Async.Parallel
    |> Async.Ignore
    |> Async.RunSynchronously
