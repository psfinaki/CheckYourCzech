module Storage.ExerciseModels.VerbImperative

open Common
open Storage.Defaults
open Storage.Storage
open BaseEntity

type VerbImperative(model: Exercises.VerbImperative) =

    inherit BaseEntity(model.Id)

    new() = VerbImperative(Exercises.VerbImperative.Default)

    [<SerializeObject>] member val Indicative = model.Indicative with get, set
    [<SerializeObject>] member val Imperatives = model.Imperatives with get, set
    [<SerializeOption>] member val Class = model.Class with get, set
    [<SerializeOption>] member val Pattern = model.Pattern with get, set
