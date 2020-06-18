module Core.IntegrationTests.NeuterNounPatternDetectorTests

open Xunit

open Core.Nouns.NeuterNounPatternDetector
open Common.WikiArticles
open WikiParsing.Articles.NounArticle

let getDeclension =
    getArticle
    >> NounArticle
    >> getDeclension

[<Theory>]
[<InlineData "okno">]
[<InlineData "slovo">]
let ``Detects pattern město`` word =
    word
    |> getDeclension
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
[<InlineData "muzeum">]
let ``Detects not pattern město`` word =
    word
    |> getDeclension
    |> isPatternMěsto
    |> Assert.False

[<Theory>]
[<InlineData "pole">]
[<InlineData "letiště">]
[<InlineData "odpoledne">]
let ``Detects pattern moře`` word =
    word
    |> getDeclension
    |> isPatternMoře
    |> Assert.True

[<Theory>]
[<InlineData "kuře">]
[<InlineData "okno">]
[<InlineData "stavení">]
[<InlineData "slůně">]
[<InlineData "vémě">]
[<InlineData "medvídě">]
[<InlineData "muzeum">]
let ``Detects not pattern moře`` word =
    word
    |> getDeclension
    |> isPatternMoře
    |> Assert.False

[<Theory>]
[<InlineData "náměstí">]
[<InlineData "překvapení">]
let ``Detects pattern stavení`` word =
    word
    |> getDeclension
    |> isPatternStavení
    |> Assert.True
    
[<Theory>]
[<InlineData "okno">]
[<InlineData "pole">]
[<InlineData "kuře">]
let ``Detects not pattern stavení`` word =
    word
    |> getDeclension
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
    |> getDeclension
    |> isPatternKuře
    |> Assert.True

[<Theory>]
[<InlineData "pole">]
[<InlineData "stavení">]
[<InlineData "okno">]
let ``Detects not pattern kuře`` word =
    word
    |> getDeclension
    |> isPatternKuře
    |> Assert.False

[<Theory>]
[<InlineData "téma">]
[<InlineData "dilema">]
let ``Detects pattern drama`` word =
    word
    |> getDeclension
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
    |> getDeclension
    |> isPatternDrama
    |> Assert.False

[<Theory>]
[<InlineData "koloseum">]
[<InlineData "studium">]
[<InlineData "baryum">]
[<InlineData "vakuum">]
let ``Detects pattern muzeum`` word =
    word
    |> getDeclension
    |> isPatternMuzeum
    |> Assert.True

[<Theory>]
[<InlineData "pole">]
[<InlineData "okno">]
[<InlineData "centrum">]
let ``Detects not pattern muzeum`` word =
    word
    |> getDeclension
    |> isPatternMuzeum
    |> Assert.False

[<Theory>]
[<InlineData "faktum">]
[<InlineData "buly">]
[<InlineData "břímě">]
let ``Detects no patterns`` word =
    word
    |> getDeclension
    |> getPatterns
    |> Seq.isEmpty
    |> Assert.True
