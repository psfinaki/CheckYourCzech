module VerbTests

open System
open Xunit
open Verb

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Theory>]
[<InlineData "housti">]
[<InlineData "péci">]
let ``Detects archaic ending`` verb =
    verb
    |> hasArchaicEnding
    |> Assert.True

[<Fact>]
let ``Detects modern ending``() =
    "péct"
    |> hasArchaicEnding
    |> Assert.False

[<Theory>]
[<InlineData("starat se", "starat")>]
[<InlineData("vážit si", "vážit")>]
[<InlineData("spát", "spát")>]
let ``Removes reflexive`` verb nonReflexive = 
    verb
    |> removeReflexive
    |> equals nonReflexive

[<Theory>]
[<InlineData("dělá", 5)>]
[<InlineData("prosí", 4)>]
[<InlineData("kupuje", 3)>]
[<InlineData("praskne", 2)>]
[<InlineData("nése", 1)>]
let ``Gets class by third person singular`` ``third person singular`` ``class`` =
    ``third person singular``
    |> getClassByThirdPersonSingular
    |> equals ``class``

[<Fact>]
let ``Throws for invalid third person singular``() =
    let action () = getClassByThirdPersonSingular "test" |> ignore
    Assert.Throws<ArgumentException> action

[<Fact>]
let ``Detects pattern tisknout``() =
    "tisknout"
    |> isPatternTisknout
    |> Assert.True

[<Theory>]
[<InlineData "dělat">]
[<InlineData "minout">]
[<InlineData "shrnout">]
let ``Detects pattern is not tisknout`` word =
    word
    |> isPatternTisknout
    |> Assert.False

[<Theory>]
[<InlineData "minout">]
[<InlineData "shrnout">]
[<InlineData "přilnout">]
let ``Detects pattern minout`` word =
    word
    |> isPatternMinout
    |> Assert.True

[<Theory>]
[<InlineData "dělat">]
[<InlineData "tisknout">]
let ``Detects pattern is not minout`` word =
    word
    |> isPatternMinout
    |> Assert.False

[<Theory>]
[<InlineData "hlásit">]
[<InlineData "řídit">]
let ``Detects pattern prosit`` word =
    word
    |> isPatternProsit
    |> Assert.True

[<Theory>]
[<InlineData "dělat">]
[<InlineData "čistit">]
let ``Detects pattern is not prosit`` word =
    word
    |> isPatternProsit
    |> Assert.False

[<Theory>]
[<InlineData "mastit">]
[<InlineData "zvýraznit">]
let ``Detects pattern čistit`` word =
    word
    |> isPatternČistit
    |> Assert.True

[<Theory>]
[<InlineData "prosit">]
[<InlineData "dělat">]
let ``Detects pattern is not čistit`` word =
    word
    |> isPatternČistit
    |> Assert.False

[<Theory>]
[<InlineData("vést", "nést")>]
[<InlineData("příst", "číst")>]
[<InlineData("téct", "péct")>]
[<InlineData("dřít", "třít")>]
[<InlineData("prát", "brát")>]
[<InlineData("vázat", "mazat")>]
let ``Gets template class 1`` verb template =
    verb
    |> getTemplateClass1
    |> equals template

[<Fact>]
let ``Throws for invalid verb class 1``() =
    let action () = getTemplateClass1 "kreslit" |> ignore
    Assert.Throws<ArgumentException> action

[<Theory>]
[<InlineData("prasknout", "tisknout")>]
[<InlineData("hynout", "minout")>]
[<InlineData("načít", "začít")>]
let ``Gets template class 2`` verb template =
    verb
    |> getTemplateClass2
    |> equals template

[<Fact>]
let ``Throws for invalid verb class 2``() =
    let action () = getTemplateClass2 "kreslit" |> ignore
    Assert.Throws<ArgumentException> action

[<Theory>]
[<InlineData("pracovat", "kupovat")>]
[<InlineData("výt", "krýt")>]
let ``Gets template class 3`` verb template =
    verb
    |> getTemplateClass3
    |> equals template

[<Fact>]
let ``Throws for invalid verb class 3``() =
    let action () = getTemplateClass3 "kreslit" |> ignore
    Assert.Throws<ArgumentException> action

[<Theory>]
[<InlineData("nosit", "prosit")>]
[<InlineData("mastit", "čistit")>]
[<InlineData("sedět", "trpět")>]
[<InlineData("házet", "sázet")>]
let ``Gets template class 4`` verb template =
    verb
    |> getTemplateClass4
    |> equals template

[<Fact>]
let ``Throws for invalid verb class 4``() =
    let action () = getTemplateClass4 "dělat" |> ignore
    Assert.Throws<ArgumentException> action

[<Fact>]
let ``Gets template class 5``() =
    "létat"
    |> getTemplateClass5
    |> equals "dělat"

[<Fact>]
let ``Throws for invalid verb class 5``() =
    let action () = getTemplateClass5 "kreslit" |> ignore
    Assert.Throws<ArgumentException> action

[<Theory>]
[<InlineData("ohlásit", 4, "prosit")>]
[<InlineData("ohlásit se", 4, "prosit")>]
let ``Gets template by class`` verb verbClass pattern =
    getTemplateByClass verb verbClass
    |> equals pattern

