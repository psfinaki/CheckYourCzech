module App

open Logging
open System.IO

let getRandomWord() = File.ReadLines "dictionary.txt"

let run log =
    let recordWithLog word = 
        try
            log (Trace ("Processing word: " + word))
            Word.record word
            log (Trace ("Processed word: " + word))
        with e ->
            log (Exception (e, ("word", word)))
            reraise()

    getRandomWord()
    |> Seq.iter recordWithLog

