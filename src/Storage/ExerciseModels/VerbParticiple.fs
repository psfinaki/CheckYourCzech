module VerbParticiple

open Exercises
open Storage
open Defaults

type VerbParticiple(model: Exercises.VerbParticiple) =

    inherit BaseEntity.BaseEntity(model.Id)

    new() = VerbParticiple(VerbParticiple.Default)

    [<SerializeObject>] member val Infinitive = model.Infinitive with get, set
    [<SerializeObject>] member val Participles = model.Participles with get, set
    [<SerializeString>] member val Pattern = model.Pattern with get, set
                        member val IsRegular = model.IsRegular with get, set
