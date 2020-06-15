module Storage.ExerciseModels.AdjectivePlural

open Common
open Storage.Defaults
open Storage.Storage
open BaseEntity

type AdjectivePlural(model: Exercises.AdjectivePlural) =

    inherit BaseEntity(model.Id)
    
    new() = AdjectivePlural(Exercises.AdjectivePlural.Default)

    [<SerializeObject>] member val Singular = model.Singular with get, set
    [<SerializeObject>] member val Plural = model.Plural with get, set
