module VerbRegistration

open Storage
open Verb
open VerbClasses
open Conjugation

let registerVerbImperative word =
    let indicative = word |> map id serializeObject ""
    let imperatives = word |> map getImperatives serializeObject ""
    let ``class`` = word |> map getClass serializeOption<VerbClass> ""
    let pattern = word |> map getImperativePattern serializeOption<string> ""

    VerbImperative.VerbImperative(word, indicative, imperatives, ``class``, pattern)
    |> upsert "verbimperatives"

let registerVerbParticiple word = 
    let infinitive = word |> map id serializeObject ""
    let participles = word |> map getParticiples serializeObject ""
    let pattern = word |> map getParticiplePattern serializeString ""
    let isRegular = word |> map hasRegularParticiple id false

    VerbParticiple.VerbParticiple(word, infinitive, participles, pattern, isRegular)
    |> upsert "verbparticiples"

let registerVerbConjugation word =
    let infinitive = word |> map id serializeObject ""
    let pattern = word |> map getParticiplePattern serializeString ""

    let getConjugation case = map (getConjugation case) serializeObject ""
    let firstSingular = word |> getConjugation FirstSingular
    let secondSingular = word |> getConjugation SecondSingular
    let thirdSingular = word |> getConjugation ThirdSingular
    let firstPlural = word |> getConjugation FirstPlural
    let secondPlural = word |> getConjugation SecondPlural
    let thirdPlural = word |> getConjugation ThirdPlural

    VerbConjugation.VerbConjugation(
        word, infinitive, pattern, 
        firstSingular, secondSingular, thirdSingular, 
        firstPlural, secondPlural, thirdPlural)
    |> upsert "verbconjugation"
