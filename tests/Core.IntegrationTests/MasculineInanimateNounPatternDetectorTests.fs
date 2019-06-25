module MasculineInanimateNounPatternDetectorTests

open Xunit
open MasculineInanimateNounPatternDetector

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

[<Theory>]
[<InlineData "strom">]
[<InlineData "leden">]
[<InlineData "ateizmus">]
[<InlineData "game">]
[<InlineData "hardware">]
[<InlineData "hemenex">]
[<InlineData "hřeben">]
[<InlineData "stůl">]
[<InlineData "týl">]
let ``Detects pattern hrad`` word =
    word
    |> isPatternHrad
    |> Assert.True

[<Theory>]
[<InlineData "konec">]
[<InlineData "déšť">]
[<InlineData "hokej">]
let ``Detects not pattern hrad`` word =
    word
    |> isPatternHrad
    |> Assert.False

[<Theory>]
[<InlineData "konec">]
[<InlineData "déšť">]
let ``Detects pattern stroj`` word =
    word
    |> isPatternStroj
    |> Assert.True

[<Theory>]
[<InlineData "strom">]
[<InlineData "omyl">]
[<InlineData "týl">]
[<InlineData "leden">]
[<InlineData "hřeben">]
let ``Detects not pattern stroj`` word =
    word
    |> isPatternStroj
    |> Assert.False

[<Theory>]
[<InlineData "rubl">]
[<InlineData "plevel">]
[<InlineData "hospic">]
let ``Detects multiple patterns`` word =
    word
    |> getPatterns
    |> Seq.isMultiple
    |> Assert.True
