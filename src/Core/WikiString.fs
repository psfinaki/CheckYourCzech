module WikiString

open StringHelper

let formSeparators = [|'/'; ','|]
let allowedLabels  = [|"(řidč.)"|]

let isBlank s = 
    s = "" ||
    s = "—"  

let isWord = not << isBlank

let isArchaic = starts "(zastarale)"
let isColloquial = starts "(hovorově)"

let isOfficial s = 
    not <| isColloquial s &&
    not <| isArchaic s

let removeLabels = removeMany allowedLabels >> trim

let getForms =
    split formSeparators
    >> Array.map trim
    >> Array.filter isWord
    >> Array.filter isOfficial
    >> Array.map removeLabels
