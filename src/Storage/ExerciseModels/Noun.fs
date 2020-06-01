module Storage.ExerciseModels.Noun

open Common
open Storage.Defaults
open Storage.Storage
open BaseEntity

type Noun(model: Exercises.Noun) = 
    
    inherit BaseEntity(model.Id)

    new() = Noun(Exercises.Noun.Default)

    [<SerializeOption>] member val Gender = model.Gender with get, set
    [<SerializeObject>] member val Patterns = model.Patterns with get, set

    [<SerializeObject>] member val SingularNominative = model.Declension.SingularNominative with get, set
    [<SerializeObject>] member val SingularGenitive = model.Declension.SingularGenitive with get, set
    [<SerializeObject>] member val SingularDative = model.Declension.SingularDative with get, set
    [<SerializeObject>] member val SingularAccusative = model.Declension.SingularAccusative with get, set
    [<SerializeObject>] member val SingularVocative = model.Declension.SingularVocative with get, set
    [<SerializeObject>] member val SingularLocative = model.Declension.SingularLocative with get, set
    [<SerializeObject>] member val SingularInstrumental = model.Declension.SingularInstrumental with get, set
    [<SerializeObject>] member val PluralNominative = model.Declension.PluralNominative with get, set
    [<SerializeObject>] member val PluralGenitive = model.Declension.PluralGenitive with get, set
    [<SerializeObject>] member val PluralDative = model.Declension.PluralDative with get, set
    [<SerializeObject>] member val PluralAccusative = model.Declension.PluralAccusative with get, set
    [<SerializeObject>] member val PluralVocative = model.Declension.PluralVocative with get, set
    [<SerializeObject>] member val PluralLocative = model.Declension.PluralLocative with get, set
    [<SerializeObject>] member val PluralInstrumental = model.Declension.PluralInstrumental with get, set
