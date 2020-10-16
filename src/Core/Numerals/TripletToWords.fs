module Core.Numerals.TripletToWords

open Common.StringHelper
open Core.Numerals.Numbers
open Core.Numerals.Triplet

let private ``words for 1 to 9`` = 
    dict [ 1,  "jedna"
           2,  "dva"
           3,  "tři"
           4,  "čtyři"
           5,  "pět"
           6,  "šest"
           7,  "sedm"
           8,  "osm"
           9,  "devět" ]

let private ``words for 10 to 19`` = 
    dict [ 0, "deset"
           1, "jedenáct"
           2, "dvanáct"
           3, "třináct"
           4, "čtrnáct"
           5, "patnáct"
           6, "šestnáct"
           7, "sedmnáct"
           8, "osmnáct"
           9, "devatenáct" ]

let private ``words for tens`` = 
    dict [ 2, "dvacet"
           3, "třicet"
           4, "čtyřicet"
           5, "padesát"
           6, "šedesát"
           7, "sedmdesát"
           8, "osmdesát"
           9, "devadesát" ]

let private ``words for hundreds`` =
    dict [ 1, "sto"
           2, "dvě stě"
           3, "tři sta"
           4, "čtyři sta"
           5, "pět set"
           6, "šest set"
           7, "sedm set"
           8, "osm set"
           9, "devět set" ]

let rec convert triplet = 
    match (triplet.Ones, triplet.Tens, triplet.Hundreds) with
    | (Digit ones, Digit 0, Digit 0) -> 
        ``words for 1 to 9``.[ones]

    | (Digit ones, Digit 1, Digit 0) -> 
        ``words for 10 to 19``.[ones]

    | (Digit 0, Digit tens, Digit 0) -> 
        ``words for tens``.[tens]

    | (Digit ones, Digit tens, Digit 0) -> 
        let wordForTen = ``words for tens``.[tens]
        let wordForOne = ``words for 1 to 9``.[ones]
        wordForTen |> appendAfterSpace wordForOne

    | (Digit 0, Digit 0, Digit hundreds) -> 
         ``words for hundreds``.[hundreds]

    | (Digit ones, Digit tens, Digit hundreds) -> 
        let remainder = 10 * tens + ones
        let toTriplet = NumberFrom1to999 >> Triplet.create

        let wordForHundred = ``words for hundreds``.[hundreds]
        let wordsForRemainder = remainder |> toTriplet |> convert
        wordForHundred |> appendAfterSpace wordsForRemainder
