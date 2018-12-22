module Tasks

type PluralsTask = { 
    Singulars: string[]
    Plurals:  string[]
}

type AccusativesTask = { 
    Singulars: string[]
    Accusatives:  string[]
}

type ComparativesTask = { 
    Positive    : string 
    Comparatives: string[]
}

type ImperativesTask = { 
    Indicative : string 
    Imperatives: string[]
}

type ParticiplesTask = { 
    Infinitive : string 
    Participles: string[]
}
