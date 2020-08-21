module Scraper.ExerciseCreation.Adjectives

open Common
open Common.Exercises
open Common.WikiArticles
open Core.Adjectives.ComparativeBuilder
open Core.Adjectives.Comparison

let private morphologicalComparativeExists comparison = 
    comparison.Comparatives
    |> Seq.filter isMorphologicalComparison
    |> Seq.any

let private hasRegularComparatives comparison = 
    comparison.Comparatives 
    |> Seq.contains (comparison.Positive |> buildComparative)

type AdjectiveDeclension with 
    static member Create adjective =
        if adjective.Declension.IsSome 
        then Some {
                CanonicalForm = adjective.CanonicalForm
                Declension = adjective.Declension.Value
            }
        else None

type AdjectiveComparative with 
    static member Create comparison = 
        if comparison |> morphologicalComparativeExists
        then 
            Some {
                Positive = comparison.Positive
                Comparatives = comparison.Comparatives
                IsRegular = comparison |> hasRegularComparatives
            }
        else None

type AdjectivePlural with 
    static member Create declension : AdjectivePlural = {
        Singular = declension.SingularNominative
        Plural = declension.PluralNominative
    }
