module VerbRegistration

open Storage
open Verb
open Conjugation
open WikiArticles
open Exercises

let registerVerbImperative verbArticleWithImperative =
    let (VerbArticleWithImperative verbArticle) = verbArticleWithImperative
    let (VerbArticle { Title = word }) = verbArticle

    upsert "verbimperatives" (VerbImperative.VerbImperative {
        Id = word
        Indicative = word
        Imperatives = verbArticleWithImperative |> getImperatives
        Class = verbArticle |> getClass
        Pattern = verbArticle |> getImperativePattern
    })

let registerVerbParticiple verbArticleWithParticiple = 
    let (VerbArticleWithParticiple verbArticle) = verbArticleWithParticiple
    let (VerbArticle { Title = word }) = verbArticle

    upsert "verbparticiples" (VerbParticiple.VerbParticiple {
        Id = word
        Infinitive = word
        Participles = verbArticleWithParticiple |> getParticiples
        Pattern = verbArticle |> getParticiplePattern
        IsRegular =  verbArticleWithParticiple |> hasRegularParticiple
    })

let registerVerbConjugation verbConjugationArticle =
    let (VerbArticleWithConjugation verbArticle) = verbConjugationArticle
    let (VerbArticle { Title = word }) = verbArticle

    upsert "verbconjugation" (VerbConjugation.VerbConjugation {
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
