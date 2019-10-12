module Noun

let getGender = 
    NounArticle.getGender
    >> Genders.translateGender

let getPatterns = 
    NounPatterns.getPatterns 

let getDeclension case number = 
    NounArticle.getDeclension case number
