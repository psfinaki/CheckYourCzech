module AdjectiveComparative

open Microsoft.WindowsAzure.Storage.Table

let isRegular adjective =
    let theoretical = ComparativeBuilder.buildComparative adjective
    let practical = Adjective.getComparatives adjective
    practical |> Array.contains theoretical

let isValid word = 
    word |> Word.isAdjective &&
    word |> Adjective.hasComparison &&
    word |> Adjective.isPositive &&
    word |> Adjective.hasMorphologicalComparatives &&
    word |> ComparativeBuilder.canBuildComparative

let getPositive = Storage.mapSafeString id
let getComparatives = Storage.mapSafeString Adjective.getComparatives
let getRegularity = Storage.mapSafeBool isRegular

type AdjectiveComparative(word) =
    inherit TableEntity(word, word)
    
    new() = AdjectiveComparative null

    member val Positive = getPositive word with get, set
    member val Comparatives = getComparatives word with get, set
    member val IsRegular = getRegularity word with get, set

let record word =
    if 
        isValid word
    then 
        word |> AdjectiveComparative |> Storage.upsert "comparatives"
