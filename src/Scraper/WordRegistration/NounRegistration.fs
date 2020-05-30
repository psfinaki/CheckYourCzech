module NounRegistration

open Storage
open Noun
open GrammarCategories
open WikiArticles
open Exercises

let registerNoun nounArticle =
    let (NounArticle { Title = word }) = nounArticle

    upsert "nouns" (Noun.Noun {
        Id = word
        Gender = nounArticle |> getGender
        Patterns = nounArticle |> getPatterns
        Declension = {
            SingularNominative = nounArticle |> getDeclension Case.Nominative Number.Singular
            SingularGenitive = nounArticle |> getDeclension Case.Genitive Number.Singular
            SingularDative = nounArticle |> getDeclension Case.Dative Number.Singular
            SingularAccusative = nounArticle |> getDeclension Case.Accusative Number.Singular
            SingularVocative = nounArticle |> getDeclension Case.Vocative Number.Singular
            SingularLocative = nounArticle |> getDeclension Case.Locative Number.Singular
            SingularInstrumental = nounArticle |> getDeclension Case.Instrumental Number.Singular
            PluralNominative = nounArticle |> getDeclension Case.Nominative Number.Plural
            PluralGenitive = nounArticle |> getDeclension Case.Genitive Number.Plural
            PluralDative = nounArticle |> getDeclension Case.Dative Number.Plural
            PluralAccusative = nounArticle |> getDeclension Case.Accusative Number.Plural
            PluralVocative = nounArticle |> getDeclension Case.Vocative Number.Plural
            PluralLocative = nounArticle |> getDeclension Case.Locative Number.Plural
            PluralInstrumental = nounArticle |> getDeclension Case.Instrumental Number.Plural
        }
    })
