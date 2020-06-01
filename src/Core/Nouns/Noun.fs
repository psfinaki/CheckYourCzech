module Core.Nouns.Noun

open WikiParsing.Articles
open Common.GenderTranslations

let getGender = 
    NounArticle.getGender
    >> Option.map fromString

let getPatterns = 
    NounPatterns.getPatterns 

let getDeclension case number = 
    NounArticle.getDeclension case number
