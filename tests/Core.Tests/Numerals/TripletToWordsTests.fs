module Core.Tests.Numerals.TripletToWordsTests

open Xunit

open Core.Numerals
open Core.Numerals.Numbers
open Core.Numerals.TripletToWords
open Core.Tests

[<Theory>]
[<InlineData(1, "jedna")>]
[<InlineData(2, "dva")>]
[<InlineData(6, "šest")>]
[<InlineData(10, "deset")>]
[<InlineData(12, "dvanáct")>]
[<InlineData(20, "dvacet")>]
[<InlineData(42, "čtyřicet dva")>]
[<InlineData(100, "sto")>]
[<InlineData(103, "sto tři")>]
[<InlineData(254, "dvě stě padesát čtyři")>]
[<InlineData(350, "tři sta padesát")>]
[<InlineData(999, "devět set devadesát devět")>]
let ``Converts`` numeral words =
    numeral
    |> Number1to999
    |> Triplet.create
    |> convert
    |> equals words
