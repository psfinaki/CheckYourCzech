﻿module Word

open Article

let noOperationAsync = async { return () }

let registerIfValid parse register = 
    parse
    >> Option.map register 
    >> Option.defaultValue noOperationAsync

let recordCzechPartOfSpeech article = function
    | "podstatné jméno" -> [
        article |> registerIfValid NounValidation.parseNoun NounRegistration.registerNoun
      ]

    | "přídavné jméno" -> [
        article |> registerIfValid AdjectiveValidation.parseAdjectivePlural AdjectiveRegistration.registerAdjectivePlural
        article |> registerIfValid AdjectiveValidation.parseAdjectiveComparative AdjectiveRegistration.registerAdjectiveComparative
      ]
            
    | "sloveso" -> [
        article |> registerIfValid VerbValidation.parseVerbImperative VerbRegistration.registerVerbImperative
        article |> registerIfValid VerbValidation.parseVerbParticiple VerbRegistration.registerVerbParticiple
        article |> registerIfValid VerbValidation.parseVerbConjugation VerbRegistration.registerVerbConjugation
      ]

    | _ -> []
    
let getTasks article = 
    article 
    |> getPartsOfSpeech 
    |> Seq.collect (recordCzechPartOfSpeech article)

let record client =
    getArticle client
    >> Option.map getTasks
    >> Option.defaultValue Seq.empty
    >> Async.Parallel
    >> Async.Ignore
    >> Async.RunSynchronously
