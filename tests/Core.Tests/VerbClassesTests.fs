module Core.Tests.VerbClassesTests

open System
open Xunit

open Core.Verbs.VerbClasses
open Common.GrammarCategories.Verbs

[<Fact>]
let ``Gets E class by third person singular``() =
    "nése"
    |> getClassByThirdPersonSingular
    |> equals VerbClass.E

[<Fact>]
let ``Gets NE class by third person singular``() =
    "praskne"
    |> getClassByThirdPersonSingular
    |> equals VerbClass.NE

[<Fact>]
let ``Gets JE class by third person singular``() =
    "kupuje"
    |> getClassByThirdPersonSingular
    |> equals VerbClass.JE
    
[<Fact>]
let ``Gets Í class by third person singular``() =
    "prosí"
    |> getClassByThirdPersonSingular
    |> equals VerbClass.Í
    
[<Fact>]
let ``Gets Á class by third person singular``() =
    "dělá"
    |> getClassByThirdPersonSingular
    |> equals VerbClass.Á

[<Fact>]
let ``Throws for invalid third person singular``() =
    let action () = getClassByThirdPersonSingular "test" |> ignore
    Assert.Throws<ArgumentException> action
