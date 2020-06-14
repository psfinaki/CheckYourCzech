module Core.Validation.VerbValidation

open Core.Verbs.Archaisms
open Common.WikiArticles

let isValidVerb = isModern

let parseVerb article =
   if article.Title |> isValidVerb
   then Some (VerbArticle article)
   else None
