module VerbImperative

open Exercises
open Storage
open Defaults

type VerbImperative(model: Exercises.VerbImperative) =

    inherit BaseEntity.BaseEntity(model.Id)

    new() = VerbImperative(VerbImperative.Default)

    [<SerializeObject>] member val Indicative = model.Indicative with get, set
    [<SerializeObject>] member val Imperatives = model.Imperatives with get, set
    [<SerializeOption>] member val Class = model.Class with get, set
    [<SerializeOption>] member val Pattern = model.Pattern with get, set
