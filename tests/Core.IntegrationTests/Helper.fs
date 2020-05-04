[<AutoOpen>]
module Helper

open System.Net.Http
open Xunit

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)

let private Client = new HttpClient()
let getArticle = 
    Article.getArticle Client
    >> Option.get
