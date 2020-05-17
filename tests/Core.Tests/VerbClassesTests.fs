module VerbClassesTests

open System
open Xunit
open VerbClasses
open Verbs

[<Fact>]
let ``Gets E class by third person singular``() =
    "nése"
    |> getClassByThirdPersonSingular
    |> equals E

[<Fact>]
let ``Gets NE class by third person singular``() =
    "praskne"
    |> getClassByThirdPersonSingular
    |> equals NE

[<Fact>]
let ``Gets JE class by third person singular``() =
    "kupuje"
    |> getClassByThirdPersonSingular
    |> equals JE
    
[<Fact>]
let ``Gets Í class by third person singular``() =
    "prosí"
    |> getClassByThirdPersonSingular
    |> equals Í
    
[<Fact>]
let ``Gets Á class by third person singular``() =
    "dělá"
    |> getClassByThirdPersonSingular
    |> equals Á

[<Fact>]
let ``Throws for invalid third person singular``() =
    let action () = getClassByThirdPersonSingular "test" |> ignore
    Assert.Throws<ArgumentException> action
