module Core.Numerals.Numbers

open Common.StringHelper

type Digit = Digit of int
type NumberFrom1to999 = NumberFrom1to999 of int
type NumberFrom1000 = NumberFrom1000 of int

let length (number: int) = number |> string |> Seq.length

let skip n (number: int) = number |> string |> takeFrom n |> int

let take n (number: int) = number |> string |> takeFirst n |> int
