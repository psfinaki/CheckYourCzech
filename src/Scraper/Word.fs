module Scraper.Word

open Core.Validation.NounValidation
open Core.Validation.AdjectiveValidation
open Core.Validation.VerbValidation
open WikiParsing.Articles.Article
open WordRegistration.NounRegistration
open WordRegistration.AdjectiveRegistration
open WordRegistration.VerbRegistration

let noOperationAsync = async { return () }

let registerIfValid parse register = 
    parse
    >> Option.map register 
    >> Option.defaultValue noOperationAsync

let recordCzechPartOfSpeech article = function
    | "podstatné jméno" -> [
        article |> registerIfValid parseNoun registerNoun
      ]

    | "přídavné jméno" -> [
        article |> registerIfValid parseAdjectivePlural registerAdjectivePlural
        article |> registerIfValid parseAdjectiveComparative registerAdjectiveComparative
      ]
            
    | "sloveso" -> [
        article |> registerIfValid parseVerbImperative registerVerbImperative
        article |> registerIfValid parseVerbParticiple registerVerbParticiple
        article |> registerIfValid parseVerbConjugation registerVerbConjugation
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
