module Scraper.Word

open Core.Validation
open WikiParsing.Articles.Article
open WordRegistration.NounRegistration
open WordRegistration.AdjectiveRegistration
open WordRegistration.VerbRegistration

let noOperationAsync = async { return () }

let recordCzechPartOfSpeech article = function
    | "podstatné jméno" ->
        article 
        |> parseNoun
        |> Option.map registerNoun
        |> Option.defaultValue [ noOperationAsync ]

    | "přídavné jméno" -> 
        article
        |> parseAdjective
        |> Option.map registerAdjective
        |> Option.defaultValue [ noOperationAsync ]
            
    | "sloveso" ->
        article
        |> parseVerb
        |> Option.map registerVerb
        |> Option.defaultValue [ noOperationAsync ]

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
