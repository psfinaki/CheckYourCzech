module NeuterNounPatternDetectorTests

open Xunit
open NeuterNounPatternDetector

[<Theory>]
[<InlineData "okno">]
[<InlineData "slovo">]
let ``Detects pattern město`` word =
    word
    |> isPatternMěsto
    |> Assert.True

[<Theory>]
[<InlineData "pole">]
[<InlineData "stavení">]
[<InlineData "kuře">]
[<InlineData "archeon">]
[<InlineData "studio">]
[<InlineData "kakao">]
[<InlineData "adjuvans">]
let ``Detects not pattern město`` word =
    word
    |> isPatternMěsto
    |> Assert.False

[<Theory>]
[<InlineData "pole">]
[<InlineData "letiště">]
[<InlineData "odpoledne">]
let ``Detects pattern moře`` word =
    word
    |> isPatternMoře
    |> Assert.True

[<Theory>]
[<InlineData "kuře">]
[<InlineData "okno">]
[<InlineData "stavení">]
[<InlineData "slůně">]
[<InlineData "vémě">]
[<InlineData "medvídě">]
let ``Detects not pattern moře`` word =
    word
    |> isPatternMoře
    |> Assert.False

[<Theory>]
[<InlineData "náměstí">]
[<InlineData "překvapení">]
let ``Detects pattern stavení`` word =
    word
    |> isPatternStavení
    |> Assert.True
    
[<Theory>]
[<InlineData "okno">]
[<InlineData "pole">]
[<InlineData "kuře">]
let ``Detects not pattern stavení`` word =
    word
    |> isPatternStavení
    |> Assert.False
    
[<Theory>]
[<InlineData "děvče">]
[<InlineData "koště">]
[<InlineData "rajče">]
[<InlineData "páže">]
[<InlineData "štíhle">]
let ``Detects pattern kuře`` word =
    word
    |> isPatternKuře
    |> Assert.True

[<Theory>]
[<InlineData "pole">]
[<InlineData "stavení">]
[<InlineData "okno">]
let ``Detects not pattern kuře`` word =
    word
    |> isPatternKuře
    |> Assert.False

[<Theory>]
[<InlineData "téma">]
[<InlineData "dilema">]
let ``Detects pattern drama`` word =
    word
    |> isPatternDrama
    |> Assert.True

[<Theory>]
[<InlineData "pole">]
[<InlineData "stavení">]
[<InlineData "okno">]
[<InlineData "kuře">]
[<InlineData "pyžama">]
let ``Detects not pattern drama`` word =
    word
    |> isPatternDrama
    |> Assert.False

[<Theory>]
[<InlineData "faktum">]
[<InlineData "buly">]
[<InlineData "břímě">]
let ``Detects no patterns`` word =
    word
    |> getPatterns
    |> Seq.isEmpty
    |> Assert.True
