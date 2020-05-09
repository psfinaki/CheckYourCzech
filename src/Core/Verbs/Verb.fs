module Verb

open WikiArticles

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

let getConjugation = 
    VerbArticle.getConjugation

let hasRegularParticiple article = 
    let (VerbArticleWithParticiple (VerbArticle { Title = verb })) = article

    let theoretical = ParticipleBuilder.buildParticiple verb
    let practical = getParticiples article
    practical |> Seq.contains theoretical

let getParticiplePattern (VerbArticle ({ Title = verb })) = 
    ParticiplePatternDetector.getPattern verb
