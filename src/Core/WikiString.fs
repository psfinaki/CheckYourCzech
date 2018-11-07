module WikiString

open StringHelper

let isBlank s = 
    s = "" ||
    s = "—"  

let isWord = not << isBlank

let getForms =
    split '/'
    >> Array.map trim
    >> Array.filter isWord
