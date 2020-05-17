module VerbRegistration

open Storage
open Verb
open Verbs
open Conjugation
open WikiArticles

let registerVerbImperative verbArticleWithImperative =
    let (VerbArticleWithImperative verbArticle) = verbArticleWithImperative
    let (VerbArticle { Title = word }) = verbArticle

    let indicative = word |> serializeObject
    let imperatives = verbArticleWithImperative |> getImperatives |> serializeObject
    let ``class`` = verbArticle |> getClass |> serializeOption<VerbClass>
    let pattern = verbArticle |> getImperativePattern |> serializeOption<string>

    VerbImperative.VerbImperative(word, indicative, imperatives, ``class``, pattern)
    |> upsert "verbimperatives"

let registerVerbParticiple verbArticleWithParticiple = 
    let (VerbArticleWithParticiple verbArticle) = verbArticleWithParticiple
    let (VerbArticle { Title = word }) = verbArticle

    let wordId = word
    let infinitive = word |> serializeObject
    let participles = verbArticleWithParticiple |> getParticiples |> serializeObject
    let pattern = verbArticle |> getParticiplePattern |> serializeString
    let isRegular = verbArticleWithParticiple |> hasRegularParticiple

    VerbParticiple.VerbParticiple(word, infinitive, participles, pattern, isRegular)
    |> upsert "verbparticiples"

let registerVerbConjugation verbConjugationArticle =
    let (VerbArticleWithConjugation verbArticle) = verbConjugationArticle
    let (VerbArticle { Title = word }) = verbArticle

    let infinitive = word |> serializeObject
    let pattern = verbArticle |> getParticiplePattern |> serializeString

    let getConjugation case = getConjugation case >> serializeObject
    let firstSingular = verbConjugationArticle |> getConjugation FirstSingular
    let secondSingular = verbConjugationArticle |> getConjugation SecondSingular
    let thirdSingular = verbConjugationArticle |> getConjugation ThirdSingular
    let firstPlural = verbConjugationArticle |> getConjugation FirstPlural
    let secondPlural = verbConjugationArticle |> getConjugation SecondPlural
    let thirdPlural = verbConjugationArticle |> getConjugation ThirdPlural

    VerbConjugation.VerbConjugation(
        word, infinitive, pattern, 
        firstSingular, secondSingular, thirdSingular, 
        firstPlural, secondPlural, thirdPlural)
    |> upsert "verbconjugation"
