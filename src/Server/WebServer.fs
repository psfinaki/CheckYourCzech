module WebServer

open Giraffe
open Saturn
open Gender
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Http

let getWordExn (ctx: HttpContext) matchRule =
    let logger = ctx.GetLogger()

    let iterator _ = 
        let word = WordGenerator.getRandomWord()
        logger.Log(LogLevel.Information, word)
        word

    iterator
    |> Seq.initInfinite
    |> Seq.find matchRule

let tryGetWord ctx matchRule =
    try
        getWordExn ctx matchRule
        |> Some
    with
    | e ->
        None

let getWord ctx matchRule =
    let iterator _ = tryGetWord ctx matchRule

    iterator
    |> Seq.initInfinite
    |> Seq.find Option.isSome
    |> Option.get

let getPluralsTask() : HttpHandler =
    fun _ ctx -> task { 
        let genderFromQuery = ctx.GetQueryStringValue "gender"
        let gender =
            match genderFromQuery with
            | Ok g -> Some (Gender.FromString g)
            | Error _ -> None

        let isAppropriateNoun gender word = 
            let basicFilter = Word.isNoun word && Noun.hasPlural word
            let genderFilter gender = Noun.hasGender gender word
            match gender with
            | Some g -> basicFilter && (genderFilter g)
            | None   -> basicFilter

        let matchRule = isAppropriateNoun gender
        let word = getWord ctx matchRule
        return! ctx.WriteJsonAsync word
    }

let getPluralsAnswer word : HttpHandler =
    fun _ ctx -> task {
        let answer = Noun.getPlural word
        return! ctx.WriteJsonAsync answer 
    }

let getComparativesTask() : HttpHandler =
    fun _ ctx -> task { 
        let matchRule = Word.isAdjective
        let word = getWord ctx matchRule
        return! ctx.WriteJsonAsync word
    }

let getComparativesAnswer word : HttpHandler =
    fun _ ctx -> task {
        let answer = Adjective.getComparative word
        return! ctx.WriteJsonAsync answer 
    }

let webApp = scope {
    get "/api/plurals/task"           (getPluralsTask())
    getf "/api/plurals/answer/%s"     getPluralsAnswer

    get  "/api/comparatives/task"     (getComparativesTask())
    getf "/api/comparatives/answer/%s" getComparativesAnswer
}