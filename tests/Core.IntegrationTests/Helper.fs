[<AutoOpen>]
module Core.IntegrationTests.Helper

open System.Net.Http
open Xunit

open WikiParsing.Articles

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

let private Client = new HttpClient()
let getArticle = 
    Article.getArticle Client
    >> Option.get
