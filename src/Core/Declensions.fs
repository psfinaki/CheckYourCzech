module Declensions

open Article
open WikiString
open NounCategories
open StringHelper

let isIndeclinable = 
    getContent
    >> getChildPart "čeština"
    >> getChildPart "podstatné jméno"
    >> getChildrenPartsWhen (starts "skloňování")
    >> Seq.map (tryGetInfo "nesklonné")
    >> Seq.forall Option.isSome

let isNominalization (noun: string) =
    let adjectiveEndings = ['ý'; 'á'; 'é'; 'í']
    let nounEnding = Seq.last noun
    adjectiveEndings |> Seq.contains nounEnding

let isNotNominalization = not << isNominalization

let getDeclensionWiki (case: Case) number word = 
    match word with
    | _ when word |> isIndeclinable ->
        [ word ]
    | _ when word |> isEditable ->
        WikiDeclensions.getEditable (int case) number word
    | _ when word |> isLocked ->
        WikiDeclensions.getLocked (int case) number word
    | word -> 
        invalidOp ("Odd word: " + word)

let getDeclension case number = 
    getDeclensionWiki case number 
    >> Seq.collect getForms
    >> Seq.distinct

let hasSingleDeclensionForCase case number = 
    getDeclension case number
    >> Seq.hasOneElement

let hasDeclensionForCase case number = 
    getDeclension case number
    >> Seq.any
