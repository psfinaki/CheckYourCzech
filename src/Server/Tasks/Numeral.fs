module Server.Tasks.Numeral

open System
open FSharp.Control.Tasks.V2
open Giraffe
open Microsoft.AspNetCore.Http

open Server.Tasks.Utils
open Core.Numerals
open Common.Utils
open Common.Numerals

let private getRange = function
    | All         -> (0, Int32.MaxValue)
    | From0To20   -> (0, 20)
    | From0To100  -> (0, 100)
    | From0To1000 -> (0, 1000)

let private getRandomNumeral (min, max) =
    Random().Next(min, max)

let getNumeralCardinalsTask next (ctx: HttpContext) =
    task {
        let range =
            ctx.TryGetQueryStringValue "range"
            |> Option.map parseUnionCase<Range>
            |> Option.defaultValue Range.All
            |> getRange
        let number = getRandomNumeral range
        let words = NumberToWords.convert number
        let task = { Word = $"{number}"; Answers = words |> Seq.toArray }
        return! Successful.OK task next ctx
    }
