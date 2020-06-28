module Storage.ExerciseModels.VerbConjugation

open Common
open Common.Conjugation
open Storage.Defaults
open Storage.Serialization
open Storage.Storage
open BaseEntity

type VerbConjugation(id, model: Exercises.VerbConjugation) =

    inherit BaseEntity(id)
    
    new() = VerbConjugation(null, Exercises.VerbConjugation.Default)

    [<SerializeObject>] member val Infinitive = model.Infinitive with get, set
    [<SerializeOption>] member val Class = model.Class with get, set
    [<SerializeOption>] member val Pattern = model.Pattern |> Option.map ConjugationPattern.toString with get, set

    [<SerializeObject>] member val FirstSingular = model.Conjugation.FirstSingular with get, set
    [<SerializeObject>] member val SecondSingular = model.Conjugation.SecondSingular with get, set
    [<SerializeObject>] member val ThirdSingular = model.Conjugation.ThirdSingular with get, set
    [<SerializeObject>] member val FirstPlural = model.Conjugation.FirstPlural with get, set
    [<SerializeObject>] member val SecondPlural = model.Conjugation.SecondPlural with get, set
    [<SerializeObject>] member val ThirdPlural = model.Conjugation.ThirdPlural with get, set
