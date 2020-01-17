module Noun

let getGender = 
    NounArticle.getGender
    >> Option.map GenderTranslations.fromString

let getPatterns = 
    NounPatterns.getPatterns 

let getDeclension case number = 
    NounArticle.getDeclension case number
