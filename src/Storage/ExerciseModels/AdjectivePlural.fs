module AdjectivePlural

open Exercises
open Storage
open Defaults

type AdjectivePlural(model: Exercises.AdjectivePlural) =

    inherit BaseEntity.BaseEntity(model.Id)
    
    new() = AdjectivePlural(AdjectivePlural.Default)

    [<SerializeObject>] member val Singular = model.Singular with get, set
    [<SerializeObject>] member val Plural = model.Plural with get, set
