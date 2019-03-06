module App

open Logging

let run log =
    let iterator _ = WordGenerator.getRandomWord()

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

