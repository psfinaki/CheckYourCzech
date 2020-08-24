module Scraper.ExerciseCreation.Verbs

open Common
open Common.Exercises
open Common.GrammarCategories.Verbs
open Common.WikiArticles
open Core.Reflexives
open Core.Verbs
open Core.Verbs.VerbClasses
open Core.Verbs.ParticipleBuilder

let private hasRegularParticiple participle = 
    participle.Participles 
    |> Seq.contains (participle.Infinitive |> buildParticiple)

let private getClass = 
    removeReflexive
    >> getClassByThirdPersonSingular

let private getThirdPersonSingular (conjugation: VerbConjugation) = 
    conjugation.Conjugation.ThirdSingular
    |> Seq.tryExactlyOne

let private getParticiplePattern = ParticiplePatternDetector.getPattern
let private getConjugationPattern = ConjugationPatternDetector.getPattern

type VerbConjugation with 
    static member Create verb =
        verb.Conjugation |> Option.map (fun conjugation -> 
        {
            Infinitive = conjugation.Infinitive
            Class = 
                verb.Conjugation 
                |> Option.bind getThirdPersonSingular
                |> Option.map getClass
            Pattern = 
                verb.Conjugation
                |> Option.bind getThirdPersonSingular
                |> Option.bind (getConjugationPattern verb.CanonicalForm)
            Conjugation = conjugation.Conjugation
        })

type VerbImperative with 
    static member Create verb =
        verb.Imperative |> Option.map (fun imperative -> 
        {
            Indicative = imperative.Indicative
            Imperatives = imperative.Imperatives
            Class = 
                verb.Conjugation 
                |> Option.bind getThirdPersonSingular
                |> Option.map getClass
            Pattern = 
                verb.Conjugation
                |> Option.bind getThirdPersonSingular
                |> Option.bind (getConjugationPattern verb.CanonicalForm)
        })

type VerbParticiple with 
    static member Create verb =
        verb.Participle |> Option.map (fun participle ->
        {
            Infinitive = participle.Infinitive
            Participles = participle.Participles
            Pattern = verb.CanonicalForm |> getParticiplePattern
            IsRegular = participle |> hasRegularParticiple 
        })
