module Server.WebServer

open Saturn

open Server.Tasks

let webApp = router {
    get "/api/nouns/declension" Noun.getNounDeclensionTask
    get "/api/adjectives/declension" Adjective.getAdjectiveDeclensionTask
    get "/api/adjectives/comparatives" Adjective.getAdjectiveComparativesTask
    get "/api/verbs/imperatives" Verb.getVerbImperativesTask
    get "/api/verbs/participles" Verb.getVerbParticiplesTask
    get "/api/verbs/conjugation" Verb.getVerbConjugationTask
}
