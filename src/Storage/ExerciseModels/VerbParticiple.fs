module Storage.ExerciseModels.VerbParticiple

open Common
open Storage.Defaults
open Storage.Storage
open BaseEntity

type VerbParticiple(model: Exercises.VerbParticiple) =

    inherit BaseEntity(model.Id)

    new() = VerbParticiple(Exercises.VerbParticiple.Default)

    [<SerializeObject>] member val Infinitive = model.Infinitive with get, set
    [<SerializeObject>] member val Participles = model.Participles with get, set
    [<SerializeString>] member val Pattern = model.Pattern with get, set
                        member val IsRegular = model.IsRegular with get, set
