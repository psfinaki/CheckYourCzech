module NounRegistration

open Storage
open Noun
open GrammarCategories
open WikiArticles

let registerNounPlural nounArticleWithPlural =
    let (NounArticleWithPlural nounArticle) = nounArticleWithPlural
    let (NounArticle { Title = word }) = nounArticle

    let singular = word |> serializeObject
    let plurals = nounArticle |> getDeclension Case.Nominative Number.Plural |> serializeObject
    let gender = nounArticle |> getGender |> serializeOption<Gender>
    let patterns = nounArticle |> getPatterns |> serializeObject

    NounPlural.NounPlural(word, singular, plurals, gender, patterns)
    |> upsert "nounplurals"

let registerNounAccusative nounArticleWithAccusative =
    let (NounArticleWithAccusative nounArticle) = nounArticleWithAccusative
    let (NounArticle { Title = word }) = nounArticle

    let nominative = word |> serializeObject
    let accusatives = nounArticle |> getDeclension Case.Accusative Number.Singular |> serializeObject
    let gender = nounArticle |> getGender |> serializeOption<Gender>
    let patterns = nounArticle |> getPatterns |> serializeObject

    NounAccusative.NounAccusative(word, nominative, accusatives, gender, patterns)
    |> upsert "nounaccusatives"
