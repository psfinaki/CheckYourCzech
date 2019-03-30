﻿module NounPatternDetectorTests

open Xunit
open NounPatternDetector

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Theory>]
[<InlineData("syn", "syna", "pan")>]
[<InlineData("otec", "otce", "muž")>]
[<InlineData("starosta", "starosty", "předseda")>]
[<InlineData("vůdce", "vůdce", "soudce")>]
let ``Gets pattern masculine animate`` nominative genitive pattern =
    getPatternMasculineAnimate (nominative, genitive)
    |> equals (Some pattern)

[<Fact>]
let ``Detects unknown pattern for masculine animate``() =
    getPatternFeminine ("vyvolený", "vyvoleného")
    |> equals None

[<Theory>]
[<InlineData("strom", "stromu", "hrad")>]
[<InlineData("leden", "ledna", "hrad")>]
[<InlineData("konec", "konce", "stroj")>]
[<InlineData("déšť", "deště", "stroj")>]
let ``Gets pattern masculine inanimate`` nominative genitive pattern =
    getPatternMasculineInanimate (nominative, genitive)
    |> equals (Some pattern)

[<Theory>]
[<InlineData("holka", "holky", "žena")>]
[<InlineData("duše", "duše", "růže")>]
[<InlineData("labuť", "labutě", "píseň")>]
[<InlineData("láhev", "láhve", "píseň")>]
[<InlineData("noc", "noci", "kost")>]
let ``Gets pattern feminine`` nominative genitive pattern =
    getPatternFeminine (nominative, genitive)
    |> equals (Some pattern)

[<Fact>]
let ``Detects unknown pattern for feminine``() =
    getPatternFeminine ("Robertsová", "Robertsové")
    |> equals None

[<Theory>]
[<InlineData("okno", "okna", "město")>]
[<InlineData("děvče", "děvčete", "kuře")>]
[<InlineData("pole", "pole", "moře")>]
[<InlineData("cvičiště", "cvičiště", "moře")>]
[<InlineData("náměstí", "náměstí", "stavení")>]
let ``Gets pattern neuter`` nominative genitive pattern =
    getPatternNeuter (nominative, genitive)
    |> equals (Some pattern)

[<Fact>]
let ``Detects unknown pattern for neuter``() =
    getPatternFeminine ("karé", "karé")
    |> equals None