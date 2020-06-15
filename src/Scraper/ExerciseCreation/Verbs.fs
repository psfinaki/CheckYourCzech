module Scraper.ExerciseCreation.Verbs

open Common
open Common.Exercises
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
let private getImperativePattern = VerbPatternDetector.getPattern

type VerbConjugation with 
    static member Create id verb = 
        verb.Conjugation |> Option.map (fun conjugation -> 
        {
            Id = id
            Infinitive = conjugation.Infinitive
            Pattern = verb.CanonicalForm |> getParticiplePattern
            Conjugation = conjugation.Conjugation
        })

type VerbImperative with 
    static member Create id verb =
        verb.Imperative |> Option.map (fun imperative -> 
        {
            Id = id
            Indicative = imperative.Indicative
            Imperatives = imperative.Imperatives
            Class = 
                verb.Conjugation 
                |> Option.bind getThirdPersonSingular
                |> Option.map getClass
            Pattern = 
                verb.Conjugation
                |> Option.bind getThirdPersonSingular
                |> Option.bind (getImperativePattern verb.CanonicalForm)
        })

type VerbParticiple with 
    static member Create id verb =
        verb.Participle |> Option.map (fun participle ->
        {
            Id = id
            Infinitive = participle.Infinitive
            Participles = participle.Participles
            Pattern = verb.CanonicalForm |> getParticiplePattern
            IsRegular = participle |> hasRegularParticiple 
        })
