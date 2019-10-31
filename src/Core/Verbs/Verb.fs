module Verb

let getClass = 
    VerbClasses.getClass

let getImperatives = 
    VerbArticle.getImperatives

let getImperativePattern =
    VerbPatternDetector.getPattern

let getParticiples =
    VerbArticle.getParticiples

let hasRegularParticiple word = 
    let theoretical = ParticipleBuilder.buildParticiple word
    let practical = getParticiples word
    practical |> Array.contains theoretical

let getParticiplePattern = 
    ParticiplePatternDetector.getPattern

