module WikiString

open StringHelper

let formSeparators = [|'/'; ','|]

let isBlank s = 
    s = "" ||
    s = "—"  

let isWord = not << isBlank

let getForms =
    split formSeparators
    >> Array.map trim
    >> Array.filter isWord
