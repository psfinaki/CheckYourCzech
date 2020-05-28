module WebServer

open Saturn
open Tasks

let webApp = router {
    get "/api/nouns/declension" Noun.getNounDeclensionTask
    get "/api/nouns/plurals" Noun.getNounPluralsTask
    get "/api/nouns/accusatives" Noun.getNounAccusativesTask
    get "/api/adjectives/plurals" Adjective.getAdjectivePluralsTask
    get "/api/adjectives/comparatives" Adjective.getAdjectiveComparativesTask
    get "/api/verbs/imperatives" Verb.getVerbImperativesTask
    get "/api/verbs/participles" Verb.getVerbParticiplesTask
    get "/api/verbs/conjugation" Verb.getVerbConjugationTask
}
