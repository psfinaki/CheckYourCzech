module VerbTests

open Xunit
open Verb

let equals (x: string[]) (y: string[]) = Assert.Equal<string []>(x, y)

[<Fact>]
let getsImperative() = 
    "spát"
    |> getImperative
    |> equals [|"spi"|]

[<Fact>]
let getsImperativeSeveralOptions() = 
    "čistit"
    |> getImperative
    |> equals [|"čisť"; "čisti"|]