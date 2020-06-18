module Core.IntegrationTests.FeminineNounPatternDetectorTests

open Xunit

open Core.Nouns.FeminineNounPatternDetector
open Common.WikiArticles
open WikiParsing.Articles.NounArticle

let getDeclension =
    getArticle
    >> NounArticle
    >> getDeclension

[<Theory>]
[<InlineData "holka">]
[<InlineData "dača">]
let ``Detects pattern žena`` word =
    word
    |> getDeclension
    |> isPatternŽena
    |> Assert.True

[<Theory>]
[<InlineData "paranoia">]
[<InlineData "radost">]
[<InlineData "přítelkyně">]
[<InlineData "píseň">]
[<InlineData "micve">]
let ``Detects not pattern žena`` word =
    word
    |> getDeclension 
    |> isPatternŽena
    |> Assert.False

[<Theory>]
[<InlineData "duše">]
[<InlineData "ulice">]
[<InlineData "kanoe">]
[<InlineData "přítelkyně">]
[<InlineData "Anglie">]
[<InlineData "paranoia">]
let ``Detects pattern růže`` word =
    word
    |> getDeclension
    |> isPatternRůže
    |> Assert.True

[<Theory>]
[<InlineData "dívka">]
[<InlineData "píseň">]
[<InlineData "láhev">]
[<InlineData "noc">]
let ``Detects not pattern růže`` word =
    word
    |> getDeclension
    |> isPatternRůže
    |> Assert.False

[<Theory>]
[<InlineData "labuť">]
[<InlineData "láhev">]
[<InlineData "postel">]
[<InlineData "chuť">]
[<InlineData "loď">]
[<InlineData "ocel">]
[<InlineData "pouť">]
let ``Detects pattern píseň`` word =
    word
    |> getDeclension
    |> isPatternPíseň
    |> Assert.True
    
[<Theory>]
[<InlineData "noc">]
[<InlineData "duše">]
[<InlineData "hyždě">]
[<InlineData "rtuť">]
[<InlineData "žena">]
[<InlineData "směs">]
[<InlineData "lež">]
[<InlineData "lest">]
let ``Detects not pattern píseň`` word =
    word
    |> getDeclension
    |> isPatternPíseň
    |> Assert.False
    
[<Theory>]
[<InlineData "radost">]
[<InlineData "čest">]
[<InlineData "řeč">]
[<InlineData "oběť">]
[<InlineData "zeď">]
[<InlineData "lest">]
[<InlineData "paměť">]
let ``Detects pattern kost`` word =
    word
    |> getDeclension
    |> isPatternKost
    |> Assert.True

[<Theory>]
[<InlineData "žena">]
[<InlineData "láhev">]
[<InlineData "píseň">]
[<InlineData "smrt">]
[<InlineData "moc">]
[<InlineData "lež">]
[<InlineData "noc">]
[<InlineData "myš">]
let ``Detects not pattern kost`` word =
    word
    |> getDeclension
    |> isPatternKost
    |> Assert.False

[<Theory>]
[<InlineData "mysl">]
[<InlineData "odpověď">]
[<InlineData "moc">]
[<InlineData "smrt">]
[<InlineData "noc">]
let ``Detects no patterns`` word =
    word
    |> getDeclension
    |> getPatterns
    |> Seq.isEmpty
    |> Assert.True
