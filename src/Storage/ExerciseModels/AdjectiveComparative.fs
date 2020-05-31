module Storage.ExerciseModels.AdjectiveComparative

open Common
open Storage.Defaults
open Storage.Storage
open BaseEntity

type AdjectiveComparative(model: Exercises.AdjectiveComparative) =

    inherit BaseEntity(model.Id)
    
    new() = AdjectiveComparative(Exercises.AdjectiveComparative.Default)

    [<SerializeObject>] member val Positive = model.Positive with get, set
    [<SerializeObject>] member val Comparatives = model.Comparatives with get, set
                        member val IsRegular = model.IsRegular with get, set
