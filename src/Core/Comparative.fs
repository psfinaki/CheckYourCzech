module Comparative

open Microsoft.WindowsAzure.Storage.Table
open Stem

let getStem (word: string) = word.TrimEnd('í', 'ý')

let buildTheoreticalComparative word = 
    let isComparativePossible = not <| (getStem word).EndsWith "c"

    let addSuffix = function
        | stem when stem |> endsHard -> stem + "ější"
        | stem when stem |> endsSoft -> stem + "ejší"
        | _ -> invalidArg word "odd adjective"

    if isComparativePossible
    then 
        word
        |> getStem
        |> alternate
        |> addSuffix
        |> Some
    else 
        None

let isRegular word =
    let theoretical = buildTheoreticalComparative word
    let practical = Adjective.getComparatives word

    theoretical 
    |> Option.map (fun t -> practical |> Array.contains t)
    |> Option.contains true

let isValid word = 
    word |> Word.isAdjective &&
    word |> Adjective.hasComparison &&
    word |> Adjective.isPositive &&
    word |> Adjective.hasMorphologicalComparatives

let getPositive = Storage.mapSafeString id
let getComparatives = Storage.mapSafeString Adjective.getComparatives
let getRegularity = Storage.mapSafeBool isRegular

type Comparative(word) =
    inherit TableEntity(word, word)
    
    new() = Comparative null

    member val Positive = getPositive word with get, set
    member val Comparatives = getComparatives word with get, set
    member val IsRegular = getRegularity word with get, set

let record word =
    if 
        isValid word
    then 
        word |> Comparative |> Storage.upsert "comparatives"
