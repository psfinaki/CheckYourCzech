module Core.Tests.ConjugationPatternDetectorTests

open Xunit

open Common.Conjugation
open Core.Verbs.ConjugationPatternDetector

let private checkPattern<'a> (getClass: string -> 'a option) pattern =
    getClass
    >> Option.get
    >> fun x -> x.ToString()
    >> fun s -> s.ToLower()
    >> equals pattern

[<Fact>]
let ``Detects pattern tisknout``() =
    "tisknout"
    |> isPatternTisknoutNonReflexive
    |> Assert.True

[<Theory>]
[<InlineData "dělat">]
[<InlineData "minout">]
[<InlineData "shrnout">]
let ``Detects pattern is not tisknout`` word =
    word
    |> isPatternTisknoutNonReflexive
    |> Assert.False

[<Theory>]
[<InlineData "minout">]
[<InlineData "shrnout">]
[<InlineData "přilnout">]
let ``Detects pattern minout`` word =
    word
    |> isPatternMinoutNonReflexive
    |> Assert.True

[<Theory>]
[<InlineData "dělat">]
[<InlineData "tisknout">]
let ``Detects pattern is not minout`` word =
    word
    |> isPatternMinoutNonReflexive
    |> Assert.False

[<Theory>]
[<InlineData "hlásit">]
[<InlineData "řídit">]
let ``Detects pattern prosit`` word =
    word
    |> isPatternPrositNonReflexive
    |> Assert.True

[<Theory>]
[<InlineData "dělat">]
[<InlineData "čistit">]
let ``Detects pattern is not prosit`` word =
    word
    |> isPatternPrositNonReflexive
    |> Assert.False

[<Theory>]
[<InlineData "mastit">]
[<InlineData "zvýraznit">]
let ``Detects pattern čistit`` word =
    word
    |> isPatternČistitNonReflexive
    |> Assert.True

[<Theory>]
[<InlineData "prosit">]
[<InlineData "dělat">]
let ``Detects pattern is not čistit`` word =
    word
    |> isPatternČistitNonReflexive
    |> Assert.False

[<Theory>]
[<InlineData("vést", "nést")>]
[<InlineData("příst", "číst")>]
[<InlineData("téct", "péct")>]
[<InlineData("dřít", "třít")>]
[<InlineData("prát", "brát")>]
[<InlineData("vázat", "mazat")>]
let ``Gets pattern class E`` verb pattern =
    checkPattern getPatternClassE pattern verb

[<Theory>]
[<InlineData "krást">]
[<InlineData "vyjet">]
[<InlineData "zaujmout">]
[<InlineData "růst">]
[<InlineData "vézt">]
let ``Detects unknown pattern for class E`` verb =
    verb
    |> getPatternClassE
    |> equals None

[<Theory>]
[<InlineData("prasknout", "tisknout")>]
[<InlineData("hynout", "minout")>]
[<InlineData("načít", "začít")>]
let ``Gets pattern class NE`` verb pattern =
    checkPattern getPatternClassNE pattern verb

[<Theory>]
[<InlineData "dostat">]
[<InlineData "sehnat">]
let ``Detects unknown pattern for class NE`` verb =
    verb
    |> getPatternClassNE
    |> equals None

[<Theory>]
[<InlineData("pracovat", "kupovat")>]
[<InlineData("výt", "krýt")>]
let ``Gets pattern class JE`` verb pattern =
    checkPattern getPatternClassJE pattern verb

[<Theory>]
[<InlineData "zabít">]
[<InlineData "prohrát">]
[<InlineData "říct">]
[<InlineData "zout">]
[<InlineData "přispět">]
let ``Detects unknown pattern for class JE`` verb =
    verb
    |> getPatternClassJE
    |> equals None

[<Theory>]
[<InlineData("nosit", "prosit")>]
[<InlineData("mastit", "čistit")>]
[<InlineData("sedět", "trpět")>]
[<InlineData("házet", "sázet")>]
let ``Gets pattern class Í`` verb pattern =
    checkPattern getPatternClassÍ pattern verb

[<Theory>]
[<InlineData "bdít">]
[<InlineData "spát">]
[<InlineData "jíst">]
let ``Detects unknown pattern for class Í`` verb =
    verb
    |> getPatternClassÍ
    |> equals None

[<Fact>]
let ``Gets pattern class Á``() =
    "létat"
    |> getPatternClassÁ
    |> equals (Some Dělat)

[<Fact>]
let ``Detects unknown pattern for class Á``() =
    "znát"
    |> getPatternClassÁ
    |> equals None
