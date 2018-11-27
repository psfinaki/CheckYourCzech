module Stem

open System

type String with
    member this.EndsWith (values: seq<char>) = 
        values
        |> Seq.map string
        |> Seq.exists this.EndsWith
    
    member this.EndsWith (values: seq<string>) = 
        values
        |> Seq.exists this.EndsWith

    member this.ReplaceEnd (oldValue: string) newValue =
        this.Remove(this.Length - oldValue.Length) + newValue

let hardConsonants = ['d';'t';'n']
let softConsonants = ['ř';'č';'ž';'š']
let neutralHardConsonants = ['v';'m';'b';'p']
let neutralSoftConsonants = ['l';'z';'s']

let alternations = 
    dict [ ("ch", "š")
           ("sk", "št")
           ("ck", "čt")
           ("čk", "čt")
           ("h", "ž")
           ("k", "č")
           ("r", "ř") ]

let alternate (stem: string) =
    if stem.EndsWith alternations.Keys
    then
        let alternant1 = alternations.Keys |> Seq.find stem.EndsWith
        let alternant2 = alternations.[alternant1]
        stem.ReplaceEnd alternant1 alternant2
    else
        stem

let endsHard (s: string) = s.EndsWith (hardConsonants @ neutralHardConsonants)
let endsSoft (s: string) = s.EndsWith (softConsonants @ neutralSoftConsonants)
