module Core.Numerals.NumberToWords

open System

open Common.StringHelper
open Core.Numerals.Numbers
open Core.Numerals.TripletToWords

type BigNumber =
    | Thousand
    | Million
    | Billion

let private getWordsForThousands = function
    | t when Triplet.value t = 1 -> "tisíc"
    | t when Triplet.value t = 2 -> "dva tisíce"
    | t when Triplet.value t = 3 -> "tři tisíce"
    | t when Triplet.value t = 4 -> "čtyři tisíce"
    | t -> t |> convert |> appendAfterSpace "tisíc"

let private getWordsForMillions = function
    | t when Triplet.value t = 1 -> "milion"
    | t when Triplet.value t = 2 -> "dva miliony"
    | t when Triplet.value t = 3 -> "tři miliony"
    | t when Triplet.value t = 4 -> "čtyři miliony"
    | t -> t |> convert |> appendAfterSpace "milionů"

let private getWordsForBillions = function
    | t when Triplet.value t = 1 -> "miliarda"
    | t when Triplet.value t = 2 -> "dvě miliardy"
    | t when Triplet.value t = 3 -> "tři miliardy"
    | t when Triplet.value t = 4 -> "čtyři miliardy"
    | t -> t |> convert |> appendAfterSpace "miliard"

let private bigNumberBuilders =
    dict [ Thousand, getWordsForThousands 
           Million,  getWordsForMillions 
           Billion,  getWordsForBillions ]

let private getFirstTriplet number = 
    let numberOfDigits = length number
    let firstDigitsToTriplet n = take n >> NumberFrom1to999 >> Triplet.create
    
    match numberOfDigits % 3 with
    | 0 -> number |> firstDigitsToTriplet 3
    | 1 -> number |> firstDigitsToTriplet 1
    | 2 -> number |> firstDigitsToTriplet 2
    | _ -> invalidOp "Math is corrupted."

let private removeFirstTriplet number =
    let firstTripletLength = number |> getFirstTriplet |> Triplet.length
    number |> skip firstTripletLength

let private hasOnlyFirstTripletSignificant = removeFirstTriplet >> (=) 0

let private getBigNumber = function
    | NumberFrom1000 number when number < 1000000 -> Thousand
    | NumberFrom1000 number when number < 1000000000 -> Million
    | NumberFrom1000 _ -> Billion

let rec convertInner = function
    | number when number < 1000 ->
        number |> NumberFrom1to999 |> Triplet.create |> convert

    | number when number |> hasOnlyFirstTripletSignificant ->
        let firstTriplet = getFirstTriplet number
        let bigNumber = getBigNumber (NumberFrom1000 number)
        bigNumberBuilders.[bigNumber] firstTriplet

    | number ->
        let firstTriplet = getFirstTriplet number
        let bigNumber = getBigNumber (NumberFrom1000 number)
        let wordsForFirstTriplet = bigNumberBuilders.[bigNumber] firstTriplet
        
        let remainder = removeFirstTriplet number
        let wordsForRemainder = remainder |> convertInner

        wordsForFirstTriplet |> appendAfterSpace wordsForRemainder

let rec convert = function
    | number when number < 0 ->
        number
        |> Math.Abs
        |> convert
        |> Seq.map (prependBeforeSpace "minus")
    | 0 -> 
        seq { "nula" }
    | 1 ->
        seq { "jeden"; "jedna" }
    | 2 -> 
        seq { "dva"; "dvě" }
    | number -> 
        number
        |> convertInner
        |> fun x -> seq { x }
