module Core.Tests.VerbPatternDetectorTests

open Xunit

open Core.Verbs.VerbPatternDetector

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
    verb
    |> getPatternClassE
    |> equals (Some pattern)

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
    verb
    |> getPatternClassNE
    |> equals (Some pattern)

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
    verb
    |> getPatternClassJE
    |> equals (Some pattern)

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
    verb
    |> getPatternClassÍ
    |> equals (Some pattern)

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
    |> equals (Some "dělat")

[<Fact>]
let ``Detects unknown pattern for class Á``() =
    "znát"
    |> getPatternClassÁ
    |> equals None
