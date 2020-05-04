module App

open Logging
open System.IO
open System.Net.Http

let getRandomWord() = File.ReadLines "dictionary.txt"

let run log =
    let client = new HttpClient()
    let recordWithLog word = 
        try
            log (Trace ("Processing word: " + word))
            Word.record client word
            log (Trace ("Processed word: " + word))
        with e ->
            log (Exception (e, ("word", word)))
            reraise()

    getRandomWord()
    |> Seq.randomize
    |> Seq.iter recordWithLog

