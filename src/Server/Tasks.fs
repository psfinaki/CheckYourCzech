module Tasks

[<AllowNullLiteral>]
type PluralsTask(singulars, plurals) = 
    member this.Singulars = singulars
    member this.Plurals = plurals

[<AllowNullLiteral>]
type AccusativesTask(singulars, accusatives) = 
    member this.Singulars = singulars
    member this.Accusatives = accusatives

[<AllowNullLiteral>]
type ComparativesTask(positive, comparatives) = 
    member this.Task = positive
    member this.Answers = comparatives

[<AllowNullLiteral>]
type ImperativesTask(indicative, imperatives) = 
    member this.Task = indicative
    member this.Answers = imperatives

[<AllowNullLiteral>]
type ParticiplesTask(infinitive, participles) = 
    member this.Task = infinitive
    member this.Answers = participles
