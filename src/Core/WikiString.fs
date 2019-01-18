module WikiString

open StringHelper

let formSeparators = [|'/'; ','|]

let allowedLabels  = [
    "(řidč.)"
    "(knižně)"
    ]

let rejectedLabels = [
    "(zastarale)"
    "(hovorově)"
    "(v obecném jazyce)"
    "(archaicky)"
    ]

let referencePattern = "\[(p )?\d*\]"

let isBlank s = 
    s = "" ||
    s = "—"  

let isWord = not << isBlank
let isOfficial = not << containsAny rejectedLabels

let removeLabels = removeMany allowedLabels >> trim
let removeReferences = removePattern referencePattern >> trim

let getForms =
    split formSeparators
    >> Array.map trim
    >> Array.filter isWord
    >> Array.filter isOfficial
    >> Array.map removeLabels
    >> Array.map removeReferences

let getForm =
    getForms
    >> Seq.exactlyOne
