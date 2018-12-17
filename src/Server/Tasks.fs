module Tasks

[<AllowNullLiteral>]
type PluralsTask(singular, plurals) = 
    member this.Singular = singular
    member this.Plurals = plurals

[<AllowNullLiteral>]
type AccusativesTask(singular, accusatives) = 
    member this.Singular = singular
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