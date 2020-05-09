module WikiString

open StringHelper

type private Form = {
    Label: string option
    Word: string
}

let private formSeparators = [|'/'; ','|]
let private referencePattern = "\[(p )?\d*\]"
let private allowedLabels  = [
     "řidč."
     "zřídka"
     "knižně"
    ]

let private isBlank s = s = "" || s = "—" || s = "-"
let private meansUnused = ends "*"
let private isPlainWord = isMatch "^\w+[\s\-]?\w+$"
let private isSpaced s = s |> starts " " || s |> ends " " 
let private hasReferences = contains "["
let private hasLabel = starts "("
let private hasSecondFormAfterColon = contains ":"
let private hasSplitForms = containsAny (formSeparators |> Seq.map string)

let private getLabeledForm =
    ``match`` "^\((?<label>.*)\) (?<word>\w*)$"
    >> fun m ->
    { 
        Word = m.Groups.["word"].Value
        Label = Some m.Groups.["label"].Value
    }

let private getFormsWhenColon =
    ``match`` "^(?<word1>\w*) \((?<label>\w*): (?<word2>\w*)\)$"
    >> fun m -> [
    { 
      Word = m.Groups.["word1"].Value
      Label = None
    }
    {   
      Word = m.Groups.["word2"].Value
      Label = Some m.Groups.["label"].Value
    } ]

let rec private getFormsInner = function
    | string when string |> isBlank ->
        []

    | string when string |> meansUnused ->
        []

    | string when string |> isPlainWord ->
        [{ Word = string; Label = None }]

    | string when string |> isSpaced ->
        string
        |> trim
        |> getFormsInner

    | string when string |> hasReferences ->
        string
        |> removePattern referencePattern
        |> getFormsInner

    | string when string |> hasLabel ->
        [ getLabeledForm string ]

    | string when string |> hasSecondFormAfterColon ->
        getFormsWhenColon string

    | string when string |> hasSplitForms ->
        string 
        |> split formSeparators
        |> List.collect getFormsInner

    | string ->
        invalidOp ("odd string: " + string)

let private isFormAppropriate form = 
    match form.Label with
    | None -> true
    | Some label -> allowedLabels |> Seq.contains label

let getForms =
    getFormsInner
    >> Seq.filter isFormAppropriate
    >> Seq.map (fun form -> form.Word)

let getForm =
    getForms
    >> Seq.exactlyOne
