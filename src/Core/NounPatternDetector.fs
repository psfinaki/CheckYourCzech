module NounPatternDetector

open Genders
open StringHelper
open Stem
open Letters

let isPatternMěsto nominative = 
    let last = nominative |> Seq.last
    let lastButOne = nominative |> Seq.lastButOne
    last = 'o' && lastButOne |> isConsonant

let getPatternMasculineAnimate = function
    | (nominative, genitive) when genitive |> ends "a" -> Some "pan"
    | (nominative, genitive) when nominative |> endsSoft -> Some "muž"
    | (nominative, genitive) when nominative |> ends "a" -> Some "předseda"
    | (nominative, genitive) when nominative |> ends "ce" -> Some "soudce"
    | _ -> None

let getPatternMasculineInanimate = function
    | (nominative, genitive) when genitive |> ends "u" || genitive |> ends "a" -> Some "hrad"
    | (nominative, genitive) when genitive |> ends "e" || genitive |> ends "ě" -> Some "stroj"
    | _ -> None

let getPatternFeminine = function
    | (nominative, genitive) when genitive |> ends "y" -> Some "žena"
    | (nominative, genitive) when nominative |> ends "e" -> Some "růže"
    | (nominative, genitive) when genitive |> ends "e" || genitive |> ends "ě" -> Some "píseň"
    | (nominative, genitive) when genitive |> ends "i" -> Some "kost"
    | _ -> None

let getPatternNeuter = function
    | (nominative, genitive) when nominative |> isPatternMěsto -> Some "město"
    | (nominative, genitive) when genitive |> ends "ete" -> Some "kuře"
    | (nominative, genitive) when genitive |> ends "e" || genitive |> ends "ě" -> Some "moře"
    | (nominative, genitive) when genitive |> ends "í" -> Some "stavení"
    | _ -> None

let patternGenderMap =
    dict [ (MasculineAnimate, getPatternMasculineAnimate)
           (MasculineInanimate, getPatternMasculineInanimate)
           (Feminine, getPatternFeminine)
           (Neuter, getPatternNeuter) ]

let getPatternByGender gender nominative genitive = patternGenderMap.[gender] (nominative, genitive)
