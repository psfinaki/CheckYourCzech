module Core.IntegrationTests.MasculineAnimateNounPatternDetectorTests

open Xunit

open Core.Nouns.MasculineAnimateNounPatternDetector
open Common.WikiArticles
open Common
open WikiParsing.Articles.NounArticle

let getDeclension =
    getArticle
    >> NounArticle
    >> getDeclension

[<Theory>]
[<InlineData "syn">]
[<InlineData "blb">]
[<InlineData "Bohumil">]
[<InlineData "bonobo">]
[<InlineData "geolog">]
let ``Detects pattern pán`` word =
    word
    |> getDeclension
    |> isPatternPán
    |> Assert.True

[<Theory>]
[<InlineData "muž">]
[<InlineData "král">]
[<InlineData "předseda">]
[<InlineData "dárce">]
let ``Detects not pattern pán`` word =
    word
    |> getDeclension
    |> isPatternPán
    |> Assert.False

[<Theory>]
[<InlineData "otec">]
[<InlineData "král">]
[<InlineData "učitel">]
[<InlineData "tloušť">]
[<InlineData "Alois">]
[<InlineData "Andrej">]
[<InlineData "Felix">]
let ``Detects pattern muž`` word =
    word
    |> getDeclension
    |> isPatternMuž
    |> Assert.True

[<Theory>]
[<InlineData "pán">]
[<InlineData "debil">]
[<InlineData "turista">]
[<InlineData "vůdce">]
let ``Detects not pattern muž`` word =
    word
    |> getDeclension
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
    |> getDeclension
    |> isPatternPředseda
    |> Assert.True
    
[<Theory>]
[<InlineData "syn">]
[<InlineData "muž">]
[<InlineData "vůdce">]
let ``Detects not pattern předseda`` word =
    word
    |> getDeclension
    |> isPatternPředseda
    |> Assert.False
    
[<Theory>]
[<InlineData "vůdce">]
[<InlineData "dárce">]
let ``Detects pattern soudce`` word =
    word
    |> getDeclension
    |> isPatternSoudce
    |> Assert.True

[<Theory>]
[<InlineData "syn">]
[<InlineData "učitel">]
[<InlineData "kolega">]
let ``Detects not pattern soudce`` word =
    word
    |> getDeclension
    |> isPatternSoudce
    |> Assert.False

[<Theory>]
[<InlineData "Marius">]
[<InlineData "Jacques">]
[<InlineData "boss">]
let ``Detects multiple patterns`` word =
    word
    |> getDeclension
    |> getPatterns
    |> Seq.containsMultiple
    |> Assert.True

[<Theory>]
[<InlineData "Henry">]
[<InlineData "George">]
let ``Detects no patterns`` word =
    word
    |> getDeclension
    |> getPatterns
    |> Seq.isEmpty
    |> Assert.True
