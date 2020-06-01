module Scraper.WordRegistration.VerbRegistration

open Common.Conjugation
open Common.Exercises
open Common.WikiArticles
open Core.Verbs.Verb
open Storage.Storage
open Storage.ExerciseModels.VerbImperative
open Storage.ExerciseModels.VerbParticiple
open Storage.ExerciseModels.VerbConjugation

let registerVerbImperative verbArticleWithImperative =
    let (VerbArticleWithImperative verbArticle) = verbArticleWithImperative
    let (VerbArticle { Title = word }) = verbArticle

    upsert "verbimperatives" (VerbImperative {
        Id = word
        Indicative = word
        Imperatives = verbArticleWithImperative |> getImperatives
        Class = verbArticle |> getClass
        Pattern = verbArticle |> getImperativePattern
    })

let registerVerbParticiple verbArticleWithParticiple = 
    let (VerbArticleWithParticiple verbArticle) = verbArticleWithParticiple
    let (VerbArticle { Title = word }) = verbArticle

    upsert "verbparticiples" (VerbParticiple {
        Id = word
        Infinitive = word
        Participles = verbArticleWithParticiple |> getParticiples
        Pattern = verbArticle |> getParticiplePattern
        IsRegular =  verbArticleWithParticiple |> hasRegularParticiple
    })

let registerVerbConjugation verbConjugationArticle =
    let (VerbArticleWithConjugation verbArticle) = verbConjugationArticle
    let (VerbArticle { Title = word }) = verbArticle

    upsert "verbconjugation" (VerbConjugation {
        Id = word
        Infinitive = word
        Pattern =  verbArticle |> getParticiplePattern
        Conjugation = {
            FirstSingular = verbConjugationArticle |> getConjugation FirstSingular
            SecondSingular = verbConjugationArticle |> getConjugation SecondSingular
            ThirdSingular = verbConjugationArticle |> getConjugation ThirdSingular
            FirstPlural = verbConjugationArticle |> getConjugation FirstPlural
            SecondPlural = verbConjugationArticle |> getConjugation SecondPlural
            ThirdPlural = verbConjugationArticle |> getConjugation ThirdPlural
        }
    })
