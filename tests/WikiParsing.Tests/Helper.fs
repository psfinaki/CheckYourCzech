[<AutoOpen>]
module WikiParsing.Tests.Helper

open System.Net.Http
open Xunit

open WikiParsing.Articles

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)
let seqEquals (expected: 'T list) (actual: seq<'T>) = Assert.Equal<'T>(expected, Seq.toList actual)

let private Client = new HttpClient()
let getArticle = 
    Article.getArticle Client
    >> Option.get
