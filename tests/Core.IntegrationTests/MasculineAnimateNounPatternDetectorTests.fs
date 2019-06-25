module MasculineAnimateNounPatternDetectorTests

open Xunit
open MasculineAnimateNounPatternDetector

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Theory>]
[<InlineData "syn">]
[<InlineData "blb">]
[<InlineData "Bohumil">]
[<InlineData "bonobo">]
[<InlineData "geolog">]
let ``Detects pattern pan`` word =
    word
    |> isPatternPan
    |> Assert.True

[<Theory>]
[<InlineData "muž">]
[<InlineData "král">]
[<InlineData "předseda">]
[<InlineData "dárce">]
let ``Detects not pattern pan`` word =
    word
    |> isPatternPan
    |> Assert.False

[<Theory>]
[<InlineData "otec">]
[<InlineData "král">]
[<InlineData "učitel">]
[<InlineData "tloušť">]
[<InlineData "Alois">]
[<InlineData "Andrej">]
let ``Detects pattern muž`` word =
    word
    |> isPatternMuž
    |> Assert.True

[<Theory>]
[<InlineData "pan">]
[<InlineData "debil">]
[<InlineData "turista">]
[<InlineData "vůdce">]
let ``Detects not pattern muž`` word =
    word
    |> isPatternMuž
    |> Assert.False

[<Theory>]
[<InlineData "starosta">]
[<InlineData "kolega">]
[<InlineData "komunista">]
[<InlineData "gymnasta">]
[<InlineData "táta">]
[<InlineData "Honza">]
let ``Detects pattern předseda`` word =
    word
    |> isPatternPředseda
    |> Assert.True
    
[<Theory>]
[<InlineData "syn">]
[<InlineData "muž">]
[<InlineData "vůdce">]
let ``Detects not pattern předseda`` word =
    word
    |> isPatternPředseda
    |> Assert.False
    
[<Theory>]
[<InlineData "vůdce">]
[<InlineData "dárce">]
let ``Detects pattern soudce`` word =
    word
    |> isPatternSoudce
    |> Assert.True

[<Theory>]
[<InlineData "syn">]
[<InlineData "učitel">]
[<InlineData "kolega">]
let ``Detects not pattern soudce`` word =
    word
    |> isPatternSoudce
    |> Assert.False

[<Theory>]
[<InlineData "Marius">]
[<InlineData "Jacques">]
[<InlineData "boss">]
let ``Detects multiple patterns`` word =
    word
    |> getPatterns
    |> Seq.isMultiple
    |> Assert.True

[<Theory>]
[<InlineData "Henry">]
[<InlineData "George">]
let ``Detects no patterns`` word =
    word
    |> getPatterns
    |> Seq.isEmpty
    |> Assert.True
