module Server.Tasks.Noun

open FSharp.Control.Tasks.V2
open Giraffe
open Microsoft.AspNetCore.Http

open Common
open Common.Utils
open Common.GrammarCategories
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

        let number = getRandomUnion<Number>
        let case = getRandomUnion<Case>
        let declensionFilter = getDeclensionProp (number, case) >> Seq.any

        let azureFilters = [ genderFilter ] |> Seq.choose id
        let postFilters = [ patternFilter; Some declensionFilter ] |> Seq.choose id
        
        let! noun = tryGetRandomWithFilters<Noun> "nouns" azureFilters postFilters
        let getTask (noun: Noun) = 
            let canonicalForm = 
                if noun.SingularNominative |> Seq.any
                then noun.SingularNominative |> Seq.random
                else noun.PluralNominative |> Seq.random

            let answers = noun |> getDeclensionProp (number, case)
            let declension = case.ToString() + " " + number.ToString()
            let word = sprintf "(%s) %s" declension canonicalForm
            Task(word, answers)
        
        let task = noun |> Option.map getTask |> Option.toObj
        return! Successful.OK task next ctx
    }

let getNounPluralsTask next (ctx: HttpContext) =
    task {
        let genderFromQuery = ctx.GetQueryStringValue "gender"
        let genderFilter = getAzureFilter "Gender" String genderFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilterCondition (pattern: string) (noun: Noun) = noun.Patterns |> Seq.contains pattern
        let patternFilter = getPostFilter patternFilterCondition patternFromQuery

        let caseFilter (noun: Noun) = 
            noun.SingularNominative |> Seq.any &&
            noun.PluralNominative |> Seq.any

        let azureFilters = [ genderFilter ] |> Seq.choose id
        let postFilters = [ patternFilter; Some caseFilter ] |> Seq.choose id
        
        let! noun = tryGetRandomWithFilters<Noun> "nouns" azureFilters postFilters
        let getTask (noun: Noun) = 
            let singular = noun.SingularNominative |> Seq.random
            let plurals = noun.PluralNominative
            Task(singular, plurals)
        
        let task = noun |> Option.map getTask |> Option.toObj
        return! Successful.OK task next ctx
    }

let getNounAccusativesTask next (ctx : HttpContext) =
    task {
        let genderFromQuery = ctx.GetQueryStringValue "gender"
        let genderFilter = getAzureFilter "Gender" String genderFromQuery

        let patternFromQuery = ctx.GetQueryStringValue "pattern"
        let patternFilterCondition (pattern: string) (noun: Noun) = noun.Patterns |> Seq.contains pattern
        let patternFilter = getPostFilter patternFilterCondition patternFromQuery

        let caseFilter (noun: Noun) = 
            noun.SingularNominative |> Seq.any &&
            noun.SingularAccusative |> Seq.any

        let azureFilters = [ genderFilter ] |> Seq.choose id
        let postFilters = [ patternFilter; Some caseFilter ] |> Seq.choose id

        let! noun = tryGetRandomWithFilters<Noun> "nouns" azureFilters postFilters
        let getTask (noun: Noun) = 
            let singular = noun.SingularNominative |> Seq.random
            let accusatives = noun.SingularAccusative
            Task(singular, accusatives)

        let task = noun |> Option.map getTask |> Option.toObj
        return! Successful.OK task next ctx
    }
