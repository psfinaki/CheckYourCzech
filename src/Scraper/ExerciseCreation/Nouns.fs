module Scraper.ExerciseCreation.Nouns

open Common.Exercises
open Common.WikiArticles
open Core.Nouns.NounPatterns

let private getPatterns noun = 
    if noun.Gender.IsSome && noun.Declension.IsSome
    then getPatterns noun.Gender.Value noun.Declension.Value noun.Declinability 
    else Seq.empty

type NounDeclension with 
    static member Create id (noun: NounDeclension) = 
        if noun.Declension.IsSome 
        then Some {
                Id = id
                Gender = noun.Gender
                Patterns = noun |> getPatterns
                Declension = noun.Declension.Value
            }
        else None
