module Tasks

[<AllowNullLiteral>]
type PluralsTask(singular, plurals) = 
    member this.Task = singular
    member this.Answers = plurals

[<AllowNullLiteral>]
type AccusativesTask(singular, accusatives) = 
    member this.Task = singular
    member this.Answers = accusatives

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
