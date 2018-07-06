module AdjectiveTests

open Xunit
open Adjective

[<Fact>]
let getsComparative() = 
    "nový"
    |> getComparative
    |> (=) "novější"
