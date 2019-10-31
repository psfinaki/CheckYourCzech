module App

open Logging

let getRandomWord() = 
    "Speciální:Náhodná_stránka"
    |> Article.getArticleName

let run log =
    let iterator _ = getRandomWord()

    let recordWithLog word = 
        try
            log (Trace ("Processing word: " + word))
            Word.record word
            log (Trace ("Processed word: " + word))
        with e ->
            log (Exception (e, ("word", word)))
            reraise()

    iterator
    |> Seq.initInfinite
    |> Seq.iter recordWithLog

