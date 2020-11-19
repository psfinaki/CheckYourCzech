module Server.Tasks.Adjective

open FSharp.Control.Tasks.V2
open Giraffe
open Microsoft.AspNetCore.Http

open Common
open Common.GrammarCategories.Common
open Common.Utils
open Server.Tasks.Utils
open Storage.Storage
open Storage.ExerciseModels.AdjectiveDeclension
open Storage.ExerciseModels.AdjectiveComparative

let private getDeclensionProp (number, case) (adjective: AdjectiveDeclension) = 
    match (number, case) with
    | Singular, Case.Nominative -> adjective.SingularNominative
    | Singular, Case.Genitive -> adjective.SingularGenitive
    | Singular, Case.Dative -> adjective.SingularDative
    | Singular, Case.Accusative -> adjective.SingularAccusative
    | Singular, Case.Vocative -> adjective.SingularVocative
    | Singular, Case.Locative -> adjective.SingularLocative
    | Singular, Case.Instrumental -> adjective.SingularInstrumental
    | Plural, Case.Nominative -> adjective.PluralNominative
    | Plural, Case.Genitive -> adjective.PluralGenitive
    | Plural, Case.Dative -> adjective.PluralDative
    | Plural, Case.Accusative -> adjective.PluralAccusative
    | Plural, Case.Vocative -> adjective.PluralVocative
    | Plural, Case.Locative -> adjective.PluralLocative
    | Plural, Case.Instrumental -> adjective.PluralInstrumental

let getAdjectiveDeclensionTask next (ctx : HttpContext) =
    task { 
        let number =
            ctx.TryGetQueryStringValue "number"
            |> Option.map parseUnionCase<Number>
            |> Option.defaultValue getRandomUnion<Number>

        let case =
            ctx.TryGetQueryStringValue "case"
            |> Option.map parseUnionCase<Case>
            |> Option.defaultValue getRandomUnion<Case>

        let declensionFilter = getDeclensionProp (number, case) >> Seq.any
        let! adjective = tryGetRandomWithFilters<AdjectiveDeclension> "adjectivedeclension" [] [ declensionFilter ] 

        match adjective with
        | Some adjective -> 
            let getTask (adjective: AdjectiveDeclension) = 
                let canonicalForm = adjective.CanonicalForm
                let answer = adjective |> getDeclensionProp (number, case)
                let declension = $"{case} {number}"
                let word = $"({declension}) {canonicalForm}"
                { Word = word; Answers = [| answer |] }

            let task = adjective |> getTask
            return! Successful.OK task next ctx
        | None ->
            return! RequestErrors.NOT_FOUND "Not Found" next ctx
    }

let getAdjectiveComparativesTask next (ctx : HttpContext) =
    task { 
        let regularityFromQuery = ctx.GetQueryStringValue "isRegular"
        let regularityFilter = getAzureFilter "IsRegular" Bool regularityFromQuery
    
        let filters = [ regularityFilter ] |> Seq.choose id
        let! adjective = tryGetRandom<AdjectiveComparative> "adjectivecomparatives" filters

        match adjective with
        | Some adjective ->
            let getTask (adjective: AdjectiveComparative) = 
                let positive = adjective.Positive
                let comparatives = adjective.Comparatives
                { Word = positive; Answers = comparatives |> Seq.toArray }
    
            let task = adjective |> getTask
            return! Successful.OK task next ctx
        | None ->
            return! RequestErrors.NOT_FOUND "Not Found" next ctx
    }
