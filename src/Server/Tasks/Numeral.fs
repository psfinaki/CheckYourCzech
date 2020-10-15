module Server.Tasks.Numeral

open System
open FSharp.Control.Tasks.V2
open Giraffe
open Microsoft.AspNetCore.Http

open Server.Tasks.Utils
open Core.Numerals
open Common.Utils
open Common.Numerals

let getMinMaxRange = function
    | From0         -> (0, Int32.MaxValue)
    | From0To100    -> (0, 100)
    | From100To1000 -> (100, 1000)
    | From1000To1000000 -> 
        (1000, 1000000)
    | From1000000 -> (1000000, Int32.MaxValue)

let getRandomNumeral (min, max) =
    Random().Next(min, max)

let getNumeralCardinalsTask next (ctx: HttpContext) =
    task {
        let range =
            ctx.TryGetQueryStringValue "range"
            |> Option.map parseUnionCase<Range>
            |> Option.defaultValue Range.From0
        let minMaxRange = getMinMaxRange range
        let number = getRandomNumeral minMaxRange
        let words = NumberToWords.convert number
        let task = { Word = string number; Answers = words |> Seq.toArray }
        return! Successful.OK task next ctx
    }
