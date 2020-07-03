module Core.Tests.ConjugationPatternDetectorTests

open Xunit

open Common.Conjugation
open Core.Verbs.ConjugationPatternDetector

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

[<Fact>]
let ``Class E - detects pattern Nést``() =
    "vést"
    |> getPatternClassE
    |> equals (Some Nést)

[<Fact>]
let ``Class E - detects pattern Číst``() =
    "příst"
    |> getPatternClassE
    |> equals (Some Číst)

[<Fact>]
let ``Class E - detects pattern Péct``() =
    "téct"
    |> getPatternClassE
    |> equals (Some Péct)

[<Fact>]
let ``Class E - detects pattern Třít``() =
    "dřít"
    |> getPatternClassE
    |> equals (Some Třít)

[<Fact>]
let ``Class E - detects pattern Brát``() =
    "prát"
    |> getPatternClassE
    |> equals (Some Brát)

[<Fact>]
let ``Class E - detects pattern Mazat``() =
    "vázat"
    |> getPatternClassE
    |> equals (Some Mazat)

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

[<Fact>]
let ``Class NE - detects pattern Tisknout``() =
    "prasknout"
    |> getPatternClassNE
    |> equals (Some Tisknout)

[<Fact>]
let ``Class NE - detects pattern Minout``() =
    "hynout"
    |> getPatternClassNE
    |> equals (Some Minout)

[<Fact>]
let ``Class NE - detects pattern Začít``() =
    "načít"
    |> getPatternClassNE
    |> equals (Some Začít)

[<Theory>]
[<InlineData "dostat">]
[<InlineData "sehnat">]
[<InlineData "milovat">]
[<InlineData "znát">]
let ``Detects unknown pattern for class NE`` verb =
    verb
    |> getPatternClassNE
    |> equals None

[<Fact>]
let ``Class JE - detects pattern Kupovat``() =
    "pracovat"
    |> getPatternClassJE
    |> equals (Some Kupovat)

[<Fact>]
let ``Class JE - detects pattern Krýt``() =
    "výt"
    |> getPatternClassJE
    |> equals (Some Krýt)

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

[<Fact>]
let ``Class Í - detects pattern Prosit``() =
    "nosit"
    |> getPatternClassÍ
    |> equals (Some Prosit)

[<Fact>]
let ``Class Í - detects pattern Čistit``() =
    "mastit"
    |> getPatternClassÍ
    |> equals (Some Čistit)

[<Fact>]
let ``Class Í - detects pattern Trpět``() =
    "sedět"
    |> getPatternClassÍ
    |> equals (Some Trpět)

[<Fact>]
let ``Class Í - detects pattern Sázet``() =
    "házet"
    |> getPatternClassÍ
    |> equals (Some Sázet)

[<Theory>]
[<InlineData "bdít">]
[<InlineData "spát">]
[<InlineData "jíst">]
[<InlineData "znát">]
let ``Detects unknown pattern for class Í`` verb =
    verb
    |> getPatternClassÍ
    |> equals None

[<Fact>]
let ``Gets pattern class Á``() =
    "létat"
    |> getPatternClassÁ
    |> equals (Some Dělat)

[<Theory>]
[<InlineData "hlásit">]
[<InlineData "kynout">]
[<InlineData "vidět">]
[<InlineData "znát">]
let ``Detects unknown pattern for class Á`` verb =
    verb
    |> getPatternClassÁ
    |> equals None
