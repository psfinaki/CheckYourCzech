module Server.Tasks.Noun

open FSharp.Control.Tasks.V2
open Giraffe
open Microsoft.AspNetCore.Http

open Common
open Common.Utils
open Common.GrammarCategories.Common
open Server.Tasks.Utils
open Storage.Storage
open Storage.ExerciseModels.Noun

let private getDeclensionProp (number, case) (noun: Noun) = 
    match (number, case) with
    | Singular, Case.Nominative -> noun.SingularNominative
    | Singular, Case.Genitive -> noun.SingularGenitive
    | Singular, Case.Dative -> noun.SingularDative
    | Singular, Case.Accusative -> noun.SingularAccusative
    | Singular, Case.Vocative -> noun.SingularVocative
    | Singular, Case.Locative -> noun.SingularLocative
    | Singular, Case.Instrumental -> noun.SingularInstrumental
    | Plural, Case.Nominative -> noun.PluralNominative
    | Plural, Case.Genitive -> noun.PluralGenitive
    | Plural, Case.Dative -> noun.PluralDative
    | Plural, Case.Accusative -> noun.PluralAccusative
    | Plural, Case.Vocative -> noun.PluralVocative
    | Plural, Case.Locative -> noun.PluralLocative
    | Plural, Case.Instrumental -> noun.PluralInstrumental

let getNounDeclensionTask next (ctx: HttpContext) =
    task {
        let genderFromQuery = ctx.GetQueryStringValue "gender"
        let genderFilter = getAzureFilter "Gender" String genderFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilterCondition (pattern: string) (noun: Noun) = noun.Patterns |> Seq.contains pattern
        let patternFilter = getPostFilter patternFilterCondition patternFromQuery

        let number =
            ctx.TryGetQueryStringValue "number"
            |> Option.map parseUnionCase<Number>
            |> Option.defaultValue getRandomUnion<Number>

        let case =
            ctx.TryGetQueryStringValue "case"
            |> Option.map parseUnionCase<Case>
            |> Option.defaultValue getRandomUnion<Case>

        let declensionFilter = getDeclensionProp (number, case) >> Seq.any

        let azureFilters = [ genderFilter ] |> Seq.choose id
        let postFilters = [ patternFilter; Some declensionFilter ] |> Seq.choose id
        
        let! noun = tryGetRandomWithFilters<Noun> "nouns" azureFilters postFilters
        match noun with
        | Some noun ->
            let getTask (noun: Noun) = 
                let canonicalForm = noun.CanonicalForm
                let answers = noun |> getDeclensionProp (number, case)
                let declension = case.ToString() + " " + number.ToString()
                let word = sprintf "(%s) %s" declension canonicalForm
                { Word = word; Answers = answers |> Seq.toArray }
        
            let task = noun |> getTask
            return! Successful. OK task next ctx
        | None ->
            return! RequestErrors.NOT_FOUND "Not Found" next ctx
    }
