module Core.Numerals.Numbers

open Common.StringHelper

type Digit = Digit of int
type Number1to999 = Number1to999 of int
type Number1000On = Number1000On of int

let length (number: int) = number |> string |> Seq.length

let skip n (number: int) = number |> string |> takeFrom n |> int

let take n (number: int) = number |> string |> takeFirst n |> int
