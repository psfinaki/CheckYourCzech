module AdjectiveComparative

open Exercises
open Storage
open Defaults

type AdjectiveComparative(model: Exercises.AdjectiveComparative) =

    inherit BaseEntity.BaseEntity(model.Id)
    
    new() = AdjectiveComparative(AdjectiveComparative.Default)

    [<SerializeObject>] member val Positive = model.Positive with get, set
    [<SerializeObject>] member val Comparatives = model.Comparatives with get, set
                        member val IsRegular = model.IsRegular with get, set
