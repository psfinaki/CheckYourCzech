module Storage.ExerciseModels.VerbImperative

open Common
open Common.GrammarCategories.Verbs
open Storage.Defaults
open Storage.Serialization
open Storage.Storage
open BaseEntity

type VerbImperative(id, model: Exercises.VerbImperative) =

    inherit BaseEntity(id)

    new() = VerbImperative(null, Exercises.VerbImperative.Default)

    [<SerializeObject>] member val Indicative = model.Indicative with get, set
    [<SerializeObject>] member val Imperatives = model.Imperatives with get, set
    [<SerializeOption>] member val Class = model.Class with get, set
    [<SerializeOption>] member val Pattern = model.Pattern |> Option.map ConjugationPattern.toString with get, set
