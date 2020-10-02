module Server.Tasks.Numeral

open System
open FSharp.Control.Tasks.V2
open Giraffe
open Microsoft.AspNetCore.Http

open Core.Numerals
open Server.Tasks.Utils
 
let getNumeralCardinalsTask next (ctx: HttpContext) =
    task {
        let number = Random().Next()
        let words = NumberToWords.convert number
        let task = { Word = string number; Answers = words |> Seq.toArray }
        return! Successful.OK task next ctx
    }
