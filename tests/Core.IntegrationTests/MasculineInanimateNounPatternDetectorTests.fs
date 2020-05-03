module MasculineInanimateNounPatternDetectorTests

open Xunit
open MasculineInanimateNounPatternDetector
open WikiArticles

let getArticle =
    getArticle
    >> NounArticle

[<Theory>]
[<InlineData "strom">]
[<InlineData "leden">]
[<InlineData "bonus">]
[<InlineData "game">]
[<InlineData "hardware">]
[<InlineData "hemenex">]
[<InlineData "hřeben">]
[<InlineData "stůl">]
[<InlineData "týl">]
let ``Detects pattern hrad`` word =
    word
    |> getArticle
    |> isPatternHrad
    |> Assert.True

[<Theory>]
[<InlineData "konec">]
[<InlineData "déšť">]
[<InlineData "hokej">]
[<InlineData "kosmos">]
let ``Detects not pattern hrad`` word =
    word
    |> getArticle
    |> isPatternHrad
    |> Assert.False

[<Theory>]
[<InlineData "konec">]
[<InlineData "déšť">]
let ``Detects pattern stroj`` word =
    word
    |> getArticle
    |> isPatternStroj
    |> Assert.True

[<Theory>]
[<InlineData "strom">]
[<InlineData "omyl">]
[<InlineData "týl">]
[<InlineData "leden">]
[<InlineData "hřeben">]
[<InlineData "kosmos">]
let ``Detects not pattern stroj`` word =
    word
    |> getArticle
    |> isPatternStroj
    |> Assert.False

[<Theory>]
[<InlineData "eukalyptus">]
[<InlineData "diabetes">]
[<InlineData "kosmos">]
let ``Detects pattern rytmus`` word =
    word
    |> getArticle
    |> isPatternRytmus
    |> Assert.True

[<Theory>]
[<InlineData "bonus">]
[<InlineData "chaos">]
[<InlineData "rádius">]
[<InlineData "hrad">]
[<InlineData "stroj">]
let ``Detects not pattern rytmus`` word =
    word
    |> getArticle
    |> isPatternRytmus
    |> Assert.False

[<Theory>]
[<InlineData "rubl">]
[<InlineData "plevel">]
[<InlineData "hospic">]
[<InlineData "glóbus">]
let ``Detects multiple patterns`` word =
    word
    |> getArticle
    |> getPatterns
    |> Seq.containsMultiple
    |> Assert.True
