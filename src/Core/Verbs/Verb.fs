module Core.Verbs.Verb

open Common.WikiArticles
open WikiParsing.Articles
open VerbClasses
open Core.Reflexives
open ParticipleBuilder

let getClass = 
    VerbArticle.getThirdPersonSingular
    >> Seq.tryExactlyOne
    >> Option.map removeReflexive
    >> Option.map getClassByThirdPersonSingular

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

    let theoretical = buildParticiple verb
    let practical = getParticiples article
    practical |> Seq.contains theoretical

let getParticiplePattern (VerbArticle ({ Title = verb })) = 
    ParticiplePatternDetector.getPattern verb
