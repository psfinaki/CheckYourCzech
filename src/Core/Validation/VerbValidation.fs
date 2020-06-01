module Core.Validation.VerbValidation

open WikiParsing.Articles.Article
open Core.Verbs.Archaisms
open Common.WikiArticles

let hasRequiredInfoParticiple = 
    isMatch [
        Is "sloveso"
        Is "časování"
    ]

let hasRequiredInfoImperative = 
    ``match`` [
        Is "sloveso"
        Is "časování"
    ]
    >> Option.map getTables
    >> Option.map (Seq.map fst)
    >> Option.map (Seq.contains "Rozkazovací způsob")
    >> Option.contains true

let hasRequiredInfoConjugation = 
    isMatch [
        Is "sloveso"
        Is "časování"
    ]

let isValidVerb = isModern

let parseVerb article =
   if article.Title |> isValidVerb
   then Some (VerbArticle article)
   else None

let parseVerbImperative =
    parseVerb
    >> Option.filter (fun (VerbArticle article) -> hasRequiredInfoImperative article)
    >> Option.map VerbArticleWithImperative

let parseVerbParticiple =
    parseVerb
    >> Option.filter (fun (VerbArticle article) -> hasRequiredInfoParticiple article)
    >> Option.map VerbArticleWithParticiple

let parseVerbConjugation = 
    parseVerb
    >> Option.filter (fun (VerbArticle article) -> hasRequiredInfoConjugation article)
    >> Option.map VerbArticleWithConjugation
