module Verb

let getClass = 
    VerbArticle.getThirdPersonSingular
    >> Seq.tryExactlyOne
    >> Option.map Reflexives.removeReflexive
    >> Option.map VerbClasses.getClassByThirdPersonSingular

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

