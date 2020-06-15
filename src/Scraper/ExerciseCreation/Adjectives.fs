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

type AdjectiveComparative with 
    static member Create id comparison = 
        if comparison |> morphologicalComparativeExists
        then 
            Some {
                Id = id
                Positive = comparison.Positive
                Comparatives = comparison.Comparatives
                IsRegular = comparison |> hasRegularComparatives
            }
        else None

type AdjectivePlural with 
    static member Create id declension = {
        Id = id
        Singular = declension.Singular
        Plural = declension.Plural
    }
