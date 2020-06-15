module Storage.ExerciseModels.VerbConjugation

open Common
open Storage.Defaults
open Storage.Storage
open BaseEntity

type VerbConjugation(model: Exercises.VerbConjugation) =

    inherit BaseEntity(model.Id)
    
    new() = VerbConjugation(Exercises.VerbConjugation.Default)

    [<SerializeObject>] member val Infinitive = model.Infinitive with get, set
    [<SerializeString>] member val Pattern = model.Pattern with get, set
    
    [<SerializeObject>] member val FirstSingular = model.Conjugation.FirstSingular with get, set
    [<SerializeObject>] member val SecondSingular = model.Conjugation.SecondSingular with get, set
    [<SerializeObject>] member val ThirdSingular = model.Conjugation.ThirdSingular with get, set
    [<SerializeObject>] member val FirstPlural = model.Conjugation.FirstPlural with get, set
    [<SerializeObject>] member val SecondPlural = model.Conjugation.SecondPlural with get, set
    [<SerializeObject>] member val ThirdPlural = model.Conjugation.ThirdPlural with get, set
