module Core.Numerals.Triplet

open Core.Numerals.Numbers

type T = {
    Ones: Digit
    Tens: Digit
    Hundreds: Digit
}

let create = function
    | NumberFrom1to999 number when number < 10 -> 
        {
            Ones = Digit number
            Tens = Digit 0
            Hundreds = Digit 0
        }
    | NumberFrom1to999 number when number < 100 ->
        {
            Ones = Digit (number % 10)
            Tens = Digit (number / 10)
            Hundreds = Digit 0
        }
    | NumberFrom1to999 number ->
        {
            Ones = Digit (number % 10)
            Tens = Digit (number / 10 % 10)
            Hundreds = Digit (number / 100)
        }

let value { Ones = Digit ones; Tens = Digit tens; Hundreds = Digit hundreds } = 
    hundreds * 100 + tens * 10 + ones

let length = value >> length
