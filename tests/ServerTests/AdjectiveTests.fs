module AdjectiveTests

open Xunit
open Adjective

let equals (x: string[]) (y: string[]) = Assert.Equal<string []>(x, y)

[<Fact>]
let getsComparative() = 
    "nový"
    |> getComparative
    |> equals [|"novější"|]

[<Fact>]
let getsComparativeSeveralOptions() = 
    "hrubý"
    |> getComparative
    |> equals [|"hrubší"; "hrubější"|]