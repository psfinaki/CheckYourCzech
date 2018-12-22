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
    member this.Positive = positive
    member this.Comparatives = comparatives

[<AllowNullLiteral>]
type ImperativesTask(indicative, imperatives) = 
    member this.Indicative = indicative
    member this.Imperatives = imperatives

[<AllowNullLiteral>]
type ParticiplesTask(infinitive, participles) = 
    member this.Infinitive = infinitive
    member this.Participles = participles