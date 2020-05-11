module NounPlural

open Exercises
open Storage
open Defaults

type NounPlural(model: Exercises.NounPlural) =

    inherit BaseEntity.BaseEntity(model.Id)
    
    new() = NounPlural(NounPlural.Default)

    [<SerializeObject>] member val Singular = model.Singular with get, set
    [<SerializeObject>] member val Plurals = model.Plurals with get, set
    [<SerializeOption>] member val Gender = model.Gender with get, set
    [<SerializeObject>] member val Patterns = model.Patterns with get, set
