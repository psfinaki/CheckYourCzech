module NounAccusative

open Exercises
open Storage
open Defaults

type NounAccusative(model: Exercises.NounAccusative) =

    inherit BaseEntity.BaseEntity(model.Id)

    new() = NounAccusative(NounAccusative.Default)

    [<SerializeObject>] member val Nominative = model.Nominative with get, set
    [<SerializeObject>] member val Accusatives = model.Accusatives with get, set
    [<SerializeOption>] member val Gender = model.Gender with get, set
    [<SerializeObject>] member val Patterns = model.Patterns with get, set
