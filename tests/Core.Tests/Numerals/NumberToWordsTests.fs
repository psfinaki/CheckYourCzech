module Core.Tests.Numerals.NumberToWordsTests

open System
open Xunit

open Core.Numerals.NumberToWords
open Core.Tests

[<Theory>]
[<InlineData(-12, "minus dvanáct")>]
[<InlineData(0, "nula")>]
[<InlineData(1, "jeden", "jedna")>]
[<InlineData(2, "dva", "dvě")>]
[<InlineData(18, "osmnáct")>]
[<InlineData(31, "třicet jedna", "jednatřicet")>]
[<InlineData(42, "čtyřicet dva", "dvaačtyřicet")>]
[<InlineData(77, "sedmdesát sedm", "sedmasedmdesát")>]
[<InlineData(1000, "tisíc")>]
[<InlineData(1003, "tisíc tři")>]
[<InlineData(1046, "tisíc čtyřicet šest")>]
[<InlineData(1070, "tisíc sedmdesát")>]
[<InlineData(1153, "tisíc sto padesát tři")>]
[<InlineData(1500, "tisíc pět set")>]
[<InlineData(2000, "dva tisíce")>]
[<InlineData(5000, "pět tisíc")>]
[<InlineData(11000, "jedenáct tisíc")>]
[<InlineData(1000000, "milion")>]
[<InlineData(3000000, "tři miliony")>]
[<InlineData(15000000, "patnáct milionů")>]
[<InlineData(200000000, "dvě stě milionů")>]
[<InlineData(1000000000, "miliarda")>]
[<InlineData(1245678903, "miliarda dvě stě čtyřicet pět milionů šest set sedmdesát osm tisíc devět set tři")>]
[<InlineData(2145678903, "dvě miliardy sto čtyřicet pět milionů šest set sedmdesát osm tisíc devět set tři")>]
let ``Converts`` numeral ([<ParamArray>] words) =
    numeral
    |> convert
    |> Seq.toArray
    |> equals words
