module Storage.ExerciseModels.AdjectivePlural

open Common
open Storage.Defaults
open Storage.Storage
open BaseEntity

type AdjectivePlural (id, model: Exercises.AdjectivePlural) =
    
    inherit BaseEntity(id)
    
    new() = AdjectivePlural(null, Exercises.AdjectivePlural.Default)

    [<SerializeObject>] member val Singular = model.Singular with get, set
    [<SerializeObject>] member val Plural = model.Plural with get, set
